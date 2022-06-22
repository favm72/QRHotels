using Common;
using SoapClient;
using DataLayer;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Security;
using Newtonsoft.Json;

namespace LogicLayer
{
    public class AccountBL : BaseBL
    {
        private MyContext context;
        private MyConfig myConfig;
        private string ErrorMessage = "Verifique sus datos. Cualquier duda llamar al anexo #9 recepción.";

        public AccountBL(MyContext context, MyConfig myConfig)
        {
            this.context = context;
            this.myConfig = myConfig;
        }
        public async Task<ResponseBE> Login(AccountBE account)
        {
            ResponseBE response = new ResponseBE();
            try
            {
                if (string.IsNullOrWhiteSpace(account.LastName))
                    throw new MyException($"Ingrese su Apellido / LastName");
                if (string.IsNullOrWhiteSpace(account.RoomCode))
                    throw new MyException($"Ingrese su Número de Habitación / Room Number");

                account.LastName = account.LastName.Trim();
                account.RoomCode = account.RoomCode.Trim();

                string HotelCode =
                    await (from h in context.Hotels
                           where h.Url == account.HotelCode
                           select h.HotelCode).FirstOrDefaultAsync();

                if (string.IsNullOrWhiteSpace(HotelCode))
                    throw new MyException($"Hotel no encontrado.");

                Reservation reserv =
                    await (from r in context.Reservation
                           where r.RoomCode == account.RoomCode
                           && r.HotelCode == HotelCode
                           && r.LastName.StartsWith(account.LastName)
                           && r.CheckIn < DateTime.Now && r.CheckOut > DateTime.Now
                           orderby r.Id descending
                           select r).FirstOrDefaultAsync();

                ServiceReference1.ResponseFindRoomReservation found = null;
                ServiceReference1.Guest guest = null;

                if (reserv == null)
                {
                    ReservationCL reservation = new ReservationCL();

                    MyRequest myRequest = new MyRequest();
                    myRequest.HotelCode = HotelCode;
                    myRequest.Username = myConfig.Username;
                    myRequest.Password = myConfig.Password;
                    myRequest.PublicKey = myConfig.PublicKey;
                    myRequest.Lastname = account.LastName;
                    myRequest.RoomCode = account.RoomCode;

                    var responseCL = await reservation.Find(myRequest);
                    if (responseCL.StatusCode != 0)
                        throw new MyException($"Status Code = {responseCL.StatusCode}.");
                    if (responseCL.Reservations.Length == 0)
                        throw new MyException($"No se encontraron reservaciones con los datos ingresados.");

                    found = responseCL.Reservations[0];
                    //guest = found.Guests.FirstOrDefault(x => x.IsTitular);
                    guest = found.Guests[0];

                    if (found.CheckOut < DateTime.Now)
                        throw new MyException($"No se encuentra actualmente hospedado.");
                }
                else if (reserv.CheckOut < DateTime.Now)
                {
                    throw new MyException($"No se encuentra actualmente hospedado.");
                }

                var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    if (reserv == null)
                    {
                        reserv = new Reservation();
                        reserv.Active = true;
                        reserv.Adults = found.Adults;
                        reserv.Childs = found.Childs;
                        reserv.Guests = found.Guests.Length;
                        reserv.CheckIn = found.CheckIn;
                        reserv.CheckOut = found.CheckOut;
                        reserv.CountryCode = guest.CountryCode;
                        reserv.CountryDescription = guest.CountryDescription;
                        reserv.Created = DateTime.Now;
                        reserv.FirstName = guest.FirstName;
                        reserv.HotelCode = found.HotelCode;
                        reserv.Age = guest.Age;
                        reserv.Email = guest.ContactEMail;
                        reserv.Document = guest.DocumentNumber;
                        reserv.DocumentType = guest.DocumentType;
                        reserv.RegisterDate = found.RegisterDate;
                        reserv.MarketSegmentCode = found.MarketSegmentCode;
                        reserv.LoyaltyCode = guest.LoyaltyCode;

                        if (guest.Client != null)
                        {
                            switch (guest.Client.Gender)
                            {
                                case ServiceReference1.EClientGender.Female:
                                    reserv.Gender = "F";
                                    break;
                                case ServiceReference1.EClientGender.Male:
                                    reserv.Gender = "M";
                                    break;
                                default:
                                    reserv.Gender = "N";
                                    break;
                            }
                            reserv.Number = guest.Client.ContactCellPhone;
                        }
                        reserv.LastName = guest.LastName;
                        reserv.ReseCode = (int)found.ReseCode;
                        reserv.ReseYear = found.ReseYear;
                        reserv.RoomCode = found.RoomCode;
                        reserv.Tipo = found.MarketOriginDescription;

                        await context.Reservation.AddAsync(reserv);
                        await context.SaveChangesAsync();
                    }

                    if (reserv.Id == 0)
                        throw new MyException("Error al obtener la reservación");

                    UserLog userLog = new UserLog();
                    userLog.IdReservation = reserv.Id;
                    userLog.Created = DateTime.Now;
                    userLog.Action = "Login";

                    await context.UserLog.AddAsync(userLog);
                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    TokenBE token = new TokenBE();
                    token.Id = reserv.Id;
                    token.Expiration = reserv.CheckOut.Value;
                    token.IsAdmin = false;

                    response.status = true;
                    response.message = "200 OK";
                    response.data = new
                    {
                        //Language = guest.LanguageISOCode,
                        Fullname = reserv.FirstName + " " + reserv.LastName,
                        RoomCode = reserv.RoomCode,
                        Guests = reserv.Guests,
                        Client = $"{reserv.ReseYear}-{reserv.ReseCode}",
                        Token = MyCrypt.Encrypt(JsonConvert.SerializeObject(token)),
                        Adults = reserv.Adults,
                        Tipo = reserv.Tipo
                    };
                }
                catch (MyException ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (MyException ex)
            {
                response.status = false;
                response.message = ex.Message;
                response.data = null;
            }
            catch (Exception)
            {
                response.status = false;
                //response.message = ex.Message;
                response.message = ErrorMessage;
                response.data = null;
            }
            return response;
        }

        public async Task<ResponseBE> AdminLogin(AccountBE account)
        {
            ResponseBE response = new ResponseBE();
            try
            {
                string encrtpted = MyCrypt.Encrypt(account.Password);
                var appuser = await (from x in context.AppUser
                                     where x.Active
                                     && x.UserName == account.UserName
                                     && x.Password == encrtpted
                                     select x).FirstOrDefaultAsync();

                if (appuser == null)
                    throw new MyException("No se encontró el usuario en Base de Datos");

                TokenBE token = new TokenBE();
                token.Id = appuser.Id;
                token.IsAdmin = true;
                token.Expiration = DateTime.Now.AddDays(2);

                response.status = true;
                response.message = "200 OK";
                response.data = new
                {
                    Name = appuser.Name,
                    Token = MyCrypt.Encrypt(JsonConvert.SerializeObject(token))
                };
            }
            catch (MyException ex)
            {
                response.status = false;
                response.message = ex.Message;
                response.data = null;
            }
            catch (Exception)
            {
                response.status = false;
                response.message = ErrorMessage;
                response.data = null;
            }
            return response;
        }

        public async Task<ResponseBE> GetHotel(string url)
        {
            ResponseBE response = new ResponseBE();
            try
            {
                var hotel = await (from h in context.Hotels
                                   from a in context.HotelAssets.Where(x => x.HotelCode == h.HotelCode).DefaultIfEmpty()
                                   where h.Url == url
                                   select new
                                   {
                                       Title = h.Title,
                                       Image = a.LoginBackground,
                                       Logo = a.Logo,
                                       LogoSmall = a.LogoSmall
                                   }).FirstOrDefaultAsync();

                if (hotel == null)
                    throw new MyException("Verifique la URL a la que está ingresando.");

                response.status = true;
                response.message = "200 OK";
                response.data = hotel;
            }
            catch (MyException ex)
            {
                response.status = false;
                response.message = ex.Message;
                response.data = null;
            }
            catch (Exception)
            {
                response.status = false;
                response.message = ErrorMessage;
                response.data = null;
            }
            return response;
        }
    }
}

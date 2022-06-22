using Common;
using SoapClient;
using DataLayer;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DataOracleLayer;

namespace LogicLayer
{
    public class InfoBL : BaseBL
    {
        private MyContext context { get; set; }
        public InfoBL(MyContext context)
        {
            this.context = context;
        }
        public async Task<ResponseBE> DirectoryServices(DummyBE dummyBE)
        {
            return await GetResponse(dummyBE, MyRole.Client, async (response) =>
            {
                var reservation = await (from r in context.Reservation
                                         where r.Active && r.Id == dummyBE.TokenBE.Id
                                         select r).FirstOrDefaultAsync();


                var list = await (from d in context.Directory
                                  where d.Active && d.HotelCode == reservation.HotelCode
                                  orderby d.OrderNo ascending
                                  select new
                                  {
                                      Name = d.Name,
                                      Content = d.Content
                                  }).ToListAsync();

                response.data = list;
            });
        }

        public async Task<ResponseBE> Schedule(DummyBE dummyBE)
        {
            return await GetResponse(dummyBE, MyRole.Client, async (response) =>
            {
                var reservation = await (from r in context.Reservation
                                         where r.Active && r.Id == dummyBE.TokenBE.Id
                                         select r).FirstOrDefaultAsync();


                var list = await (from s in context.Schedule
                                  where s.Active && s.HotelCode == reservation.HotelCode
                                  select new
                                  {
                                      Id = s.Id,
                                      Description = s.Description
                                  }).ToListAsync();

                response.data = list;
            });
        }

        public async Task<ResponseBE> Hotels(DummyBE dummyBE)
        {
            return await GetResponse(dummyBE, MyRole.Admin, async (response) =>
            {
                var list = await (from m in context.Permission
                                  join h in context.Hotels on m.HotelCode equals h.HotelCode
                                  where m.IdUser == dummyBE.TokenBE.Id
                                  select new
                                  {
                                      Id = h.HotelCode,
                                      Description = h.ShortDescription
                                  }).ToListAsync();

                response.data = list;
            });
        }

        public async Task<ResponseBE> HotelInfo(DummyBE dummyBE)
        {
            return await GetResponse(dummyBE, MyRole.Client, async (response) =>
            {
                var hotelCode = await (from r in context.Reservation
                                       where r.Active && r.Id == dummyBE.TokenBE.Id
                                       select r.HotelCode).FirstOrDefaultAsync();

                var info = await (from h in context.Hotels
                                  from a in context.HotelAssets.Where(x => x.HotelCode == h.HotelCode).DefaultIfEmpty()
                                  where h.HotelCode == hotelCode
                                  select new
                                  {
                                      hotelType = h.HotelType,
                                      ShowRestaurants = h.ShowRestaurants,
                                      ImageRestaurants = h.ImageRestaurants,
                                      Image = a.LoginBackground,
                                      Logo = a.Logo,
                                      LogoSmall = a.LogoSmall
                                  }).FirstOrDefaultAsync();

                response.data = info;
            });
        }

        public async Task<ResponseBE> Cleaning(DummyBE dummyBE)
        {
            return await GetResponse(dummyBE, MyRole.Client, async (response) =>
            {
                var hotelCode =
                await (from r in context.Reservation
                       where r.Active && r.Id == dummyBE.TokenBE.Id
                       select r.HotelCode).FirstOrDefaultAsync();

                var info =
                await (from c in context.Cleaning                       
                       where c.HotelCode == hotelCode
                       select new
                       {
                           c.Description,
                           c.Price,
                       }).FirstOrDefaultAsync();

                response.data = info;
            });
        }

        public async Task<ResponseBE> HotelCode(DummyBE dummyBE)
        {
            return await GetResponse(dummyBE, MyRole.Client, async (response) =>
            {
                var hotelCode = await (from r in context.Reservation
                                       where r.Active && r.Id == dummyBE.TokenBE.Id
                                       select r.HotelCode).FirstOrDefaultAsync();

                response.data = hotelCode;
            });
        }

        public async Task<ResponseBE> Providers(DummyBE dummyBE)
        {
            return await GetResponse(dummyBE, MyRole.Admin, async (response) =>
            {
                var list = await (from p in context.Provider
                                  select new
                                  {
                                      Id = p.Id,
                                      Description = p.Name
                                  }).ToListAsync();

                response.data = list;
            });
        }

        public async Task<ResponseBE> Slider(DummyBE dummyBE)
        {
            return await GetResponse(dummyBE, MyRole.Client, async (response) =>
            {
                var reservation = await (from r in context.Reservation
                                         where r.Active && r.Id == dummyBE.TokenBE.Id
                                         select r).FirstOrDefaultAsync();

                var list = await (from s in context.Slider
                                  where s.Active && s.HotelCode == reservation.HotelCode
                                  orderby s.OrderNo ascending
                                  select new
                                  {
                                      Title = s.Title,
                                      ImageURL = s.ImageUrl,
                                      LinkURL = s.LinkUrl
                                  }).ToListAsync();

                response.data = list;
            });
        }

        public async Task<ResponseBE> LateCheckout(DummyBE dummyBE)
        {
            return await GetResponse(dummyBE, MyRole.Client, async (response) =>
            {
                var reservation = await (from r in context.Reservation
                                         where r.Active && r.Id == dummyBE.TokenBE.Id
                                         select r).FirstOrDefaultAsync();

                var list = await (from l in context.LateCheckout
                                  where l.Active && l.HotelCode == reservation.HotelCode
                                  orderby l.HourLimit ascending
                                  select new
                                  {
                                      Id = l.Id,
                                      HourLimit = l.HourLimit,
                                      Description = l.Description,
                                      ConfirmText = l.ConfirmText,
                                      Price = l.Price
                                  }).ToListAsync();

                response.data = list;
            });
        }

        public async Task<bool> isUnlimited(DummyBE dummyBE, string conn_ora)
        {
            bool unlimitedCred = false;
            try
            {
                var w = await (from r in context.Reservation
                               join h in context.Hotels on r.HotelCode equals h.HotelCode
                               where r.Active && r.Id == dummyBE.TokenBE.Id
                               select new
                               {
                                   RMC = h.Rmc,
                                   ReseCode = r.ReseCode,
                                   ReseYear = r.ReseYear
                               }).FirstOrDefaultAsync();

                var oracleContext = new OracleContext(conn_ora);
                unlimitedCred = await oracleContext.isCreditUnlimited(w.RMC, w.ReseCode.ToString(), w.ReseYear.ToString());
            }
            catch (Exception)
            {
                unlimitedCred = true;
            }
            return unlimitedCred;
        }

        public async Task<ResponseBE> PaymTypes(DummyBE dummyBE, string conn_ora)
        {
            return await GetResponse(dummyBE, MyRole.Client, async (response) =>
            {
                bool unlimitedCred = await isUnlimited(dummyBE, conn_ora);

                var list = await (from t in context.PaymType
                                  where t.Active && (unlimitedCred || t.UnlimitedCred != true)
                                  select new
                                  {
                                      Id = t.Id,
                                      Name = t.Name
                                  }).ToListAsync();

                response.data = list;
            });
        }
    }
}

using ServiceReference1;
using System;
using System.Threading.Tasks;

namespace SoapClient
{	
	public class ReservationCL
	{
		public async Task<ResponseFindReservation> Find(MyRequest myRequest)
		{		
			WSNewHotelSrvSoapClient client = new WSNewHotelSrvSoapClient(WSNewHotelSrvSoapClient.EndpointConfiguration.WSNewHotelSrvSoap);

			RqFindReservation request = new RqFindReservation();
			request.PublicKey = myRequest.PublicKey;
			request.Language = "ES";
			request.HotelCode = myRequest.HotelCode;
			request.Username = myRequest.Username;
			request.Password = myRequest.Password;
			request.StateInformation = new RqStateInformation();
			request.StateInformation.Reserved = false;
			request.StateInformation.CheckIn = true;
			request.StateInformation.CheckOut = false;
			request.StateInformation.Canceled = false;
			request.StateInformation.NoShow = false;
			request.StateInformation.Overbooking = false;
			request.GuestInformation = new RqGuestInformation();
			request.GuestInformation.ClientData = true;
			request.GuestInformation.LastName = new CompareStrInformation();
			request.GuestInformation.LastName.CompareStrOperator = "1";
			request.GuestInformation.LastName.Value = myRequest.Lastname;
			request.GuestInformation.LastName.MatchCase = false;
			request.RoomInformation = new RqRoomInformation();
			request.RoomInformation.RoomCode = myRequest.RoomCode;
			ResponseFindReservation response = await client.FindReservationsAsync(request);

			return response;
		}
	}
}

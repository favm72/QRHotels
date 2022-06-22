namespace Common
{
    public class InfoPageBE : IHasToken
	{
		public string Token { get; set; }
		public int Id { get; set; }
		public string HotelCode { get; set; }		
		public string Name { get; set; }
		public TokenBE TokenBE { get; set; }
	}
}

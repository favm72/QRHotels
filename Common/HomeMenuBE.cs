namespace Common
{
    public class HomeMenuBE : IHasToken
	{
		public string Token { get; set; }
		public int Id { get; set; }
		public string HotelCode { get; set; }
		public string Description { get; set; }
		public string Title { get; set; }
		public string ImageUrl { get; set; }
		public string LinkUrl { get; set; }
		public string Link { get; set; }
		public int? IdInfoPage { get; set; }
		public int OrderNo { get; set; }
		public bool IsHalf { get; set; }
		public bool Active { get; set; }
		public string Image64 { get; set; }
		public string Filename { get; set; }
		public TokenBE TokenBE { get; set; }
	}
}

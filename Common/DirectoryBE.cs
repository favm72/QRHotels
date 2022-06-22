namespace Common
{
    public class DirectoryBE : IHasToken
	{
		public string Token { get; set; }
		public int Id { get; set; }
		public string HotelCode { get; set; }
		public string Name { get; set; }
		public string Introduction { get; set; }
		public string BannerUrl { get; set; }
		public string IconUrl { get; set; }
		public int OrderNo { get; set; }
		public string Content { get; set; }
		public bool Active { get; set; }
		public string Icon64 { get; set; }
		public string Iconname { get; set; }
		public string Image64 { get; set; }
		public string Filename { get; set; }
		public TokenBE TokenBE { get; set; }
	}
}

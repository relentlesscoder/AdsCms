namespace AdCms.Domain.Model
{
    public class AdsDo
    {
        public int AdId { get; set; }

        public int? BrandId { get; set; }

        public string BrandName { get; set; }

        public decimal NumPages { get; set; }

        public string Position { get; set; }
    }
}

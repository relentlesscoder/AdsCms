namespace AdCms.Core
{
    public static class Enums
    {
        public enum PageMode
        {
            Full = 1,
            GeFifty = 2,
            TopFiveAds = 3,
            TopFiveBrands = 4
        }

        public enum SortOrder
        {
            [StringValue("asc")]
            Asc = 1,
            [StringValue("desc")]
            Desc = 2
        }

        public enum ColumnIndex
        {
            [StringValue("AdId")]
            AdId = 0,
            [StringValue("BrandId")]
            BrandId = 1,
            [StringValue("BrandName")]
            Name = 2,
            [StringValue("NumPages")]
            NumPages = 3,
            [StringValue("Position")]
            Position = 4
        }
    }
}

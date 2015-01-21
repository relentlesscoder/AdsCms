using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdCms.Core
{
    public static class Enums
    {
        public enum SortOrder
        {
            Asc = 1,
            Desc = 2
        }

        public enum SortByColumn
        {
            AdId = 1,
            BrandId = 2,
            Name = 3,
            NumPages = 4,
            Position = 5
        }
    }
}

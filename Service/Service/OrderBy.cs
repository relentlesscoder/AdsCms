using System;
using System.Linq.Expressions;
using AdCms.Core;

namespace AdCme.Service.Service
{
    public class SortBy<T, TKey>
    {
        public Expression<Func<T, TKey>> KeySelector { get; set; }

        public Enums.SortOrder Order { get; set; }
    }
}

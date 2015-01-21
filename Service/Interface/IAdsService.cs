using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AdCme.Service.Service;
using AdCms.Domain.Model;

namespace AdCme.Service.Interface
{
    public interface IAdsService
    {
        List<AdsDo> GetAdsList<TKey>(DateTime startDate, DateTime endDate, IList<SortBy<AdsDo, TKey>> sortBy = null,
            Expression<Func<AdsDo, bool>> predicate = null, int numberOfItems = 0);
    }
}

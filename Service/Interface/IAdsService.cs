using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AdCme.Service.Service;
using AdCms.Domain.Model;

namespace AdCme.Service.Interface
{
    public interface IAdsService
    {
        List<AdsDo> GetAdsList(DateTime startDate, DateTime endDate, ref int total, ref int filteredCount, IList<SortBy> sortBy = null,
            Expression<Func<AdsDo, bool>> predicate = null, int count = 0, int offset = 0);
        List<AdsDo> GetTopAdsByCoverage(DateTime startDate, DateTime endDate, int count = 0);
        List<AdsDo> GetTopBrandsByCoverage(DateTime startDate, DateTime endDate, int count = 0);
    }
}

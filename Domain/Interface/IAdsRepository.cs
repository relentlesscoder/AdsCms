using System;
using System.Collections.Generic;
using AdCms.Domain.Model;

namespace AdCms.Domain.Interface
{
    public interface IAdsRepository
    {
        List<AdsDo> GetAdsByDateRange(DateTime startDate, DateTime endDate);
    }
}

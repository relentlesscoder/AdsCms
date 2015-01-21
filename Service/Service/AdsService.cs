using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AdCme.Service.Interface;
using AdCms.Core;
using AdCms.Domain.Interface;
using AdCms.Domain.Model;

namespace AdCme.Service.Service
{
    public class AdsService : IAdsService
    {
        #region Members

        private readonly IAdsRepository _adsRepository;

        #endregion

        #region Constructors

        public AdsService(IAdsRepository adsRepository)
        {
            if (adsRepository == null)
            {
                //ToDo: logging
            }
            this._adsRepository = adsRepository;
        }

        #endregion

        #region IAdsService Members

        public List<AdsDo> GetAdsList<TKey>(DateTime startDate, DateTime endDate, ref int total, IList<SortBy<AdsDo, TKey>> sortBy = null,
            Expression<Func<AdsDo, bool>> predicate = null, int count = 0, int offset = 0)
        {
            List<AdsDo> adsDos = _adsRepository.GetAdsByDateRange(startDate, endDate);
            total = adsDos.Count;

            IQueryable<AdsDo> entityList;
            if (predicate != null)
            {
                entityList = (adsDos.AsQueryable()).Where(predicate);
            }
            else
            {
                entityList = adsDos.AsQueryable();
            }
            if (sortBy != null && sortBy.Count > 0)
            {
                entityList = ApplySortCriterion(entityList, sortBy[0]);
                for (int i = 1; i < sortBy.Count; i++)
                {
                    entityList = ApplySortCriterion(entityList, sortBy[i]);
                }
            }
            else
            {
                entityList = entityList.OrderBy(p => p.BrandName);
            }

            if (offset > 0)
            {
                entityList = entityList.Skip(offset);
            }
            if (count > 0)
            {
                entityList = entityList.Take(count);
            }

            return entityList.ToList();
        }

        #endregion

        #region Private Methods

        private IQueryable<AdsDo> ApplySortCriterion<TKey>(IQueryable<AdsDo> queryable,
            SortBy<AdsDo, TKey> sortCriterion)

    {
        IQueryable<AdsDo> orderedEntityList;
        switch (sortCriterion.Order)
        {
            case Enums.SortOrder.Desc:
                orderedEntityList = queryable.OrderByDescending(sortCriterion.KeySelector);
                break;
            default:
                orderedEntityList = queryable.OrderBy(sortCriterion.KeySelector);
                break;
        }
        return orderedEntityList;
    }

        #endregion
    }
}

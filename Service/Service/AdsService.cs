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

        public List<AdsDo> GetAdsList<TKey>(DateTime startDate, DateTime endDate, IList<SortBy<AdsDo, TKey>> sortBy = null,
            Expression<Func<AdsDo, bool>> predicate = null, int numberOfItems = 0)
        {
            List<AdsDo> adsDos = _adsRepository.GetAdsByDateRange(startDate, endDate);

            IQueryable<AdsDo> entityList;
            IOrderedQueryable<AdsDo> orderedEntityList;
            if (predicate != null)
            {
                entityList = (adsDos.AsQueryable()).Where(predicate);
            }
            else
            {
                entityList = adsDos.AsQueryable();
            }
            if (numberOfItems > 0)
            {
                entityList = entityList.Take(numberOfItems);
            }
            if (sortBy != null && sortBy.Count > 0)
            {
                orderedEntityList = ApplySortCriterion(entityList, sortBy[0]);
                for (int i = 1; i < sortBy.Count; i++)
                {
                    orderedEntityList = ApplySortCriterion(orderedEntityList, sortBy[i]);
                }
            }
            else
            {
                orderedEntityList = entityList.OrderBy(p => p.BrandName);
            }

            return orderedEntityList.ToList();
        }

        #endregion

        #region Private Methods

        private IOrderedQueryable<AdsDo> ApplySortCriterion<TKey>(IQueryable<AdsDo> queryable,
            SortBy<AdsDo, TKey> sortCriterion)

    {
        IOrderedQueryable<AdsDo> orderedEntityList;
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

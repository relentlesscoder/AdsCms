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
                throw new ArgumentException("IAdsRepository required");
            }
            this._adsRepository = adsRepository;
        }

        #endregion

        #region IAdsService Members

        public List<AdsDo> GetAdsList(DateTime startDate, DateTime endDate, ref int total, ref int filteredCount, IList<SortBy> sortBy = null,
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

            filteredCount = (entityList.ToList()).Count;
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

        public List<AdsDo> GetTopAdsByCoverage(DateTime startDate, DateTime endDate,
            int count = 0)
        {
            List<AdsDo> adsDos = _adsRepository.GetAdsByDateRange(startDate, endDate);

            var result =
                adsDos
                    .OrderByDescending(p => p.NumPages)
                    .ThenBy(p => p.BrandName).Take(count);

            return result.ToList();
        }

        public List<AdsDo> GetTopBrandsByCoverage(DateTime startDate, DateTime endDate,
            int count = 0)
        {
            List<AdsDo> adsDos = _adsRepository.GetAdsByDateRange(startDate, endDate);

            var result =
                adsDos.GroupBy(p => p.BrandId)
                    .Select(
                        p =>
                            new AdsDo
                            {
                                AdId = -1,
                                BrandId = p.First().BrandId,
                                BrandName = p.First().BrandName,
                                NumPages = p.Sum(c => c.NumPages),
                                Position = "N/A"
                            })
                    .OrderByDescending(p => p.NumPages)
                    .ThenBy(p => p.BrandName).Take(count);
            return result.ToList();
        }

        #endregion

        #region Private Methods

        private IQueryable<AdsDo> ApplySortCriterion(IQueryable<AdsDo> queryable,
            SortBy sortCriterion)

    {
        IQueryable<AdsDo> orderedEntityList;
        switch (sortCriterion.Order)
        {
            case Enums.SortOrder.Desc:
                orderedEntityList = queryable.OrderByDescending(OrderExpression<AdsDo>(sortCriterion.Name));
                break;
            default:
                orderedEntityList = queryable.OrderBy(OrderExpression<AdsDo>(sortCriterion.Name));
                break;
        }
        return orderedEntityList;
    }

        public static Expression<Func<T, object>> OrderExpression<T>(string sortBy)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "item");

            Expression<Func<T, object>> sortExpression = Expression.Lambda<Func<T, object>>
                (Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

            return sortExpression;
        }

        #endregion
    }
}

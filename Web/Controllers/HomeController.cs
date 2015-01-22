using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Web.Mvc;
using AdCme.Service.Interface;
using AdCme.Service.Service;
using AdCms.Core;
using AdCms.Domain.Model;

namespace AdCms.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Members

        private readonly IAdsService _adsService;
        private readonly DateTime _startDate = new DateTime(2011, 1, 1);
        private readonly DateTime _endDate = new DateTime(2011, 4, 1);
        private const string ORDER_BY_COL_PATTERN = "order[{index}][column]";
        private const string ORDER_BY_DIR_PATTERN = "order[{index}][dir]";

        #endregion

        #region Constructors

        public HomeController(IAdsService adsService)
        {
            if (adsService == null)
            {
                throw new ArgumentException("IAdsService required");
            }

            this._adsService = adsService;
        }

        #endregion

        #region Action Methods

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult DisplayAds(int mode, int draw, int start, int length)
        {
            int totalCount = 0;
            int filteredCount = 0;
            Expression<Func<AdsDo, bool>> predicate = GetPredictByPageMode(mode);
            List<SortBy> sortBy = ParseSortByParameters(HttpContext.Request.QueryString);
            List<AdsDo> adsDos = null;
            switch ((Enums.PageMode)mode)
            {
                case Enums.PageMode.Full:
                case Enums.PageMode.GeFifty:
                    adsDos = _adsService.GetAdsList(_startDate, _endDate, ref totalCount, ref filteredCount, sortBy,
                        count: length,
                        offset: start, predicate: predicate);
                    break;
                case Enums.PageMode.TopFiveAds:
                    adsDos = _adsService.GetTopAdsByCoverage(_startDate, _endDate, 5);
                    totalCount = filteredCount = adsDos.Count;
                    break;
                case Enums.PageMode.TopFiveBrands:
                    adsDos = _adsService.GetTopBrandsByCoverage(_startDate, _endDate, 5);
                    totalCount = filteredCount = adsDos.Count;
                    break;
            }
            List<List<String>> data = MapToAdsData(adsDos);
            return Json(new {draw = draw, recordsTotal = totalCount, recordsFiltered = filteredCount, data = data},
                JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods

        private List<SortBy> ParseSortByParameters(NameValueCollection queryStrings)
        {
            List<SortBy> sortBys = new List<SortBy>();
            foreach (int columnIndex in Enum.GetValues(typeof(Enums.ColumnIndex)))
            {
                string column = ORDER_BY_COL_PATTERN.Replace("{index}", columnIndex.ToString());
                string direction = ORDER_BY_DIR_PATTERN.Replace("{index}", columnIndex.ToString());
                if (!String.IsNullOrEmpty(queryStrings[column]) && !String.IsNullOrEmpty(queryStrings[direction]))
                {
                    SortBy sortBy = new SortBy();
                    sortBy.Order = (Enums.SortOrder)StringEnum.Parse(typeof(Enums.SortOrder), queryStrings[direction]);
                    sortBy.Name = StringEnum.GetStringValue((Enums.ColumnIndex)Convert.ToInt32(queryStrings[column]));
                    sortBys.Add(sortBy);
                }
            }

            return sortBys;
        }

        private Expression<Func<AdsDo, bool>> GetPredictByPageMode(int mode)
        {
            Expression<Func<AdsDo, bool>> predicate;
            switch ((Enums.PageMode)mode)
            {
                case Enums.PageMode.GeFifty:
                    predicate = @do =>  @do.NumPages >= 0.5m && String.CompareOrdinal(@do.Position, "Cover") == 0;
                    break;
                default:
                    predicate = null;
                    break;
            }
            return predicate;
        }

        private List<List<String>> MapToAdsData(IList<AdsDo> adsDos)
        {
            List<List<String>> data = new List<List<string>>();

            foreach (AdsDo adsDo in adsDos)
            {
                List<String> attributeList = new List<string>();
                attributeList.Add(adsDo.AdId <= 0 ? "N/A" : adsDo.AdId.ToString());
                attributeList.Add(adsDo.BrandId.ToString());
                attributeList.Add(adsDo.BrandName);
                attributeList.Add(adsDo.NumPages.ToString());
                attributeList.Add(adsDo.Position);
                data.Add(attributeList);
            }

            return data;
        }

        #endregion
    }
}

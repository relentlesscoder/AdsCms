using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AdCme.Service.Interface;
using AdCms.Domain.Model;
using AdCms.Web.Models;

namespace AdCms.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Members

        private readonly IAdsService _adsService;
        private readonly DateTime _startDate = new DateTime(2011, 1, 1);
        private readonly DateTime _endDate = new DateTime(2011, 4, 1);
        private const int DEFAULT_ITEM_PER_PAGE = 10;

        #endregion

        #region Constructors

        public HomeController(IAdsService adsService)
        {
            if (adsService == null)
            {
                //ToDo: logging
            }

            this._adsService = adsService;
        }

        #endregion

        #region Action Methods

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult DisplayAds(int draw, int start, int length)
        {
            int totalCount = 0;
            List<AdsDo> adsDos = _adsService.GetAdsList<string>(_startDate, _endDate, ref totalCount, count:length, offset:start);
            List<List<String>> data = MapToAdsData(adsDos);
            return Json(new { draw = draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = data }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods

        private AdsViewModel MapToAdsViewModel(IList<AdsDo> adsDos)
        {
            AdsViewModel adsViewModel = new AdsViewModel();

            adsViewModel.Ads = adsDos;

            return adsViewModel;
        }

        private List<List<String>> MapToAdsData(IList<AdsDo> adsDos)
        {
            List<List<String>> data = new List<List<string>>();

            foreach (AdsDo adsDo in adsDos)
            {
                List<String> attributeList = new List<string>();
                attributeList.Add(adsDo.AdId.ToString());
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

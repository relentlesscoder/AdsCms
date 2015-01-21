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
        private const int ITEM_PER_PAGE = 100;

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

        public ActionResult DisplayAds()
        {
            List<AdsDo> adsDos = _adsService.GetAdsList<string>(_startDate, _endDate);
            AdsViewModel viewModel = MapToAdsViewModel(adsDos);
            return PartialView("DisplayAds", viewModel);
        }

        public ActionResult DisplayAdsByCriterion()
        {
            AdsViewModel viewModel = MapToAdsViewModel(null);
            return PartialView("DisplayAds", viewModel);
        }

        public ActionResult DisplayTopFiveAdsByCoverage()
        {
            AdsViewModel viewModel = MapToAdsViewModel(null);
            return PartialView("DisplayAds", viewModel);
        }

        public ActionResult DisplayTopFiveBrandsByCoverage()
        {
            AdsViewModel viewModel = MapToAdsViewModel(null);
            return PartialView("DisplayAds", viewModel);
        }

        #endregion

        #region Private Methods

        private AdsViewModel MapToAdsViewModel(IList<AdsDo> adsDos)
        {
            AdsViewModel adsViewModel = new AdsViewModel();

            adsViewModel.Ads = adsDos;

            return adsViewModel;
        }

        #endregion
    }
}

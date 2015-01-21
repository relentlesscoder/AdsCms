using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            //ToDo: get data and map to view model
            AdsViewModel viewModel = new AdsViewModel();
            return View(viewModel);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}

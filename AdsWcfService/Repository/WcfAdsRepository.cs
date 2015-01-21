using System;
using System.Collections.Generic;
using AdCms.AdsWcfService.AdServiceReference;
using AdCms.Domain.Interface;
using AdCms.Domain.Model;

namespace AdCms.AdsWcfService.Repository
{
    public class WcfAdsRepository : IAdsRepository
    {
        #region Constructor

        public WcfAdsRepository(){}

        #endregion

        #region IAdsRepository Members

        public List<AdsDo> GetAdsByDateRange(DateTime startDate, DateTime endDate)
        {
            List<AdsDo> adsDos = new List<AdsDo>();
            try
            {
                using (AdDataServiceClient proxy = new AdDataServiceClient())
                {
                    Ad[] ads = proxy.GetAdDataByDateRange(startDate, endDate);
                    // Maps WCF response Ad objects to the domain AdsDo object
                    if (ads != null && ads.Length > 0)
                    {
                        foreach (Ad ad in ads)
                        {
                            if (ad != null)
                            {
                                AdsDo adsDo = MapToDomainAdsDo(ad);
                                adsDos.Add(adsDo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ToDo: logging
            }
            return adsDos;
        }

        #endregion

        #region Private Methods

        private AdsDo MapToDomainAdsDo(Ad ad)
        {
            AdsDo adsDo = new AdsDo
            {
                AdId = ad.AdId,
                NumPages = ad.NumPages,
                Position = ad.Position
            };

            if (ad.Brand != null)
            {
                adsDo.BrandId = ad.Brand.BrandId;
                adsDo.BrandName = ad.Brand.BrandName;
            }
            else
            {
                adsDo.BrandId = null;
                adsDo.BrandName = null;
            }
            return adsDo;
        }

        #endregion
    }
}

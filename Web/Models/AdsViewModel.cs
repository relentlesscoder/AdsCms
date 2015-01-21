using System.Collections.Generic;
using AdCms.Domain.Model;

namespace AdCms.Web.Models
{
    public class AdsViewModel
    {
        public IList<AdsDo> Ads { get; set; }
    }
}
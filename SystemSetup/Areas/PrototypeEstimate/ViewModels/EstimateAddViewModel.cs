using iEnterAsia.iseiQ.Areas.PrototypeEstimate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iEnterAsia.iseiQ.Areas.PrototypeEstimate.ViewModels
{
    public class EstimateAddViewModel
    {
        public Estimate Estimate { get; set; }

        public IList<SelectListItem> closing_site_type_list { get; set; }

        

    }
}
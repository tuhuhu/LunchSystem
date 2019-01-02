using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using iEnterAsia.iseiQ.Models;
using iEnterAsia.iseiQ.Areas.PrototypeEstimate.Models;

namespace iEnterAsia.iseiQ.Areas.PrototypeEstimate.ViewModels
{
    public partial class EstimateIndexViewModel : IIndexViewModel
    {
        public Estimate Estimate { get; set; }
        public List<Estimate> Estimates { get; set; }

        public List<dynamic> FindAll()
        {
            var results = new List<dynamic>();
            this.Estimates.ForEach(x => results.Add(x));
            return results;
        }

        public IViewModel Find()
        {
            return new EstimateViewModel(this.Estimate);
        }

        public dynamic FindForSummary()
        {
            return null;
        }

        public dynamic FindForDetail()
        {
            return null;
        }
    }
    
}


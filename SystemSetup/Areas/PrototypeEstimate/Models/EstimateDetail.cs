using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iEnterAsia.iseiQ.Areas.PrototypeEstimate.Models
{
    public class EstimateDetail
    {
        [DisplayName("品名")]
        public string ProductName { get; set; }

        [DisplayName("単価")]
        public decimal UnitPrice { get; set; }

        [DisplayName("数量")]
        public decimal Quantity { get; set; }

        [DisplayName("単位")]
        public string UnitType { get; set; }

        [DisplayName("金額")]
        public decimal Amount { get; set; }
    }
}
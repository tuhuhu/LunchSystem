using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class CategorySetDuplicateModel: BaseEntity
    {
        public string SOURCE_COMPANY_CODE { get; set; }

        public string DESTINATION_COMPANY_CODE { get; set; }

        public IList<SelectListItem> SOURCE_COMPANY_CODE_LIST { get; set; }
        public IList<SelectListItem> DESTINATION_COMPANY_CODE_LIST { get; set; }

    }
}

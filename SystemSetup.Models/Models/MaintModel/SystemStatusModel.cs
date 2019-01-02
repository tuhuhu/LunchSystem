using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class SystemStatusModel : SystemStatusEntityPlus
    {
        // 運転モードリスト
        public IEnumerable<SelectListItem> SYSTEM_OPERATION_MODE_LIST { get; set; }
    }
}

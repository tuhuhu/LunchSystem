using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SystemSetup.Models
{
    public class AllInformationListModel : AllInformationEntity
    {
        public AllInformationEntity REGIST_PARAM { get; set; }

        public string CONTENT_DATE { get; set; }
        public string START_DATE { get; set; }
        public string END_DATE { get; set; }

        public AllInformationEntity SEARCH_PARAM { get; set; }

        public AllInformationListModel INFO_LIST { get; set; }

        public string SEARCH_START { get; set; }
        public string SEARCH_LENGTH { get; set; }

    }
}
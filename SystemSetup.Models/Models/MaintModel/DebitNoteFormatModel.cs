using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemSetup.Models;

namespace SystemSetup.Models
{
    public class DebitNoteFormatModel : DebitNoteFormatEntity
    {
        public IList<SelectListItem> CONTRACT_FIRM_LIST { get; set; }

        public IList<DebitNoteFormatEntity> DEBIT_NOTE_FORMAT_LIST { get; set; }

        public string SEARCH_COMPANY_CD { get; set; }

    }
}

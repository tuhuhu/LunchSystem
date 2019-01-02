using System;

namespace iEnterAsia.iseiQ.Models
{
    public class TransInformationListModel : InformationEntity
    {
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public bool INCLUDE_DELETED { get; set; }

        public TransInformationListModel()
        {
            INCLUDE_DELETED = false;
        }
    }
}

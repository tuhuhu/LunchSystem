using System;

namespace SystemSetup.Models
{
    public class PostInformationListModel : InformationEntity
    {
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public bool INCLUDE_DELETED { get; set; }

        public PostInformationListModel()
        {
            INCLUDE_DELETED = false;
        }
    }
}

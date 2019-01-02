using System;

namespace SystemSetup.Models
{
    public class OrgMaintListModel : OrgMaintEntity
    {
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public bool INCLUDE_DELETED { get; set; }

        public OrgMaintListModel()
        {
            INCLUDE_DELETED = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models.Models.AllInformationModel
{
    public class AllInformationModel
    {
        public IList<AllInformationEntity> INFO_LIST { get; set; }

        public AllInformationModel()
        {
            INFO_LIST = new List<AllInformationEntity>();
        }
    }
}

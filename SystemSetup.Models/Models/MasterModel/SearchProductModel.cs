//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2015/01/12
// Comment		: Create new
//------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    /// <summary>
    /// SearchCustomerModel
    /// </summary>
    public class SearchProductModel
    {
        /// <summary>
        /// Callback function name return execution result
        /// </summary>
        public string CallBack { get; set; }
        
        public int CONTRACT_TYPE { get; set; }

        public int CONTRACT_TYPE_CLASS { get; set; }

        public int PRODUCT_NO { get; set; }

        public string CONTRACT_TYPE_DISP_NAME { get; set; }

        public string CONTRACT_TYPE_CLASS_DISP_NAME { get; set; }

        public string PRODUCT_NAME { get; set; }
        
    }
}

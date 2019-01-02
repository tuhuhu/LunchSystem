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
using SystemSetup.Constants;

namespace SystemSetup.Models
{
    /// <summary>
    /// SearchCustomerModel
    /// </summary>
    public class SearchDebitAccountModel
    {
        /// <summary>
        /// Callback function name return execution result
        /// </summary>
        public string CallBack { get; set; }

        public int? TIME_INDEX { get; set; }

        public DebitAccountEntity DEBIT_INFO { get; set; }

        public SearchDebitAccountModel()
        {
            DEBIT_INFO = new DebitAccountEntity();
            TIME_INDEX = TimeIndex.ContractPeriodStart;
        }
    }
}

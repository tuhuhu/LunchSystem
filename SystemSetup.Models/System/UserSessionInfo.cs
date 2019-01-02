//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/03
// Comment		: Create new
//------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemSetup.Models
{
	public class UserSessionInfo
	{
		public string UserID { get; set; }
		public string UserName { get; set; }
		public string StoreID { get; set; }
		public string StoreName { get; set; }
		public string IPAddress { get; set; }
	}
}

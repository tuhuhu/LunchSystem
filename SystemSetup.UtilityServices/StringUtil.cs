//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/17
// Comment		: Create new
//------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemSetup.Constants;
//using SystemSetup.DataAccess;
using SystemSetup.Models;
using System.Web;
using System.Text.RegularExpressions;

namespace SystemSetup.UtilityServices
{
	public static partial class StringUtil
	{
        public static string GetHtmlTitle(string title)
        {
            return string.Format("{0} : {1}", "i-seiQ", title);
        }

        public static string GetScreenTitle(string title)
        {
            return title;
        }

        public static string RTrim(string stringValue)
		{
			if ( String.IsNullOrEmpty( stringValue ) )
			{
				return stringValue;
			}
			else
			{
				return stringValue.TrimEnd();
			}
		}

		public static string TrimString(string inputString, int lenghRequire)
		{
			if (inputString.Length > lenghRequire)
				return inputString.Remove(lenghRequire) + "...";
			else
				return inputString;
		}

	}
}

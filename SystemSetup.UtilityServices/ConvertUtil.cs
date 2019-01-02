//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/17
// Comment		: Create new
//------------------------------------------------------------------------

using System;

namespace SystemSetup.UtilityServices
{
	public static class ConvertUtil
	{
		public static int? ConvertDateStringToDateInt( string dateString )
		{
			int value;
			if ( String.IsNullOrEmpty( dateString ) )
			{
				return null;
			}

			if ( Int32.TryParse( dateString.Replace( "/", String.Empty ), out value ) )
			{
				return value;
			}
			else
			{
				return null;
			}
		}

		public static string ConvertDateIntToDateString( int dateInt )
		{
			string dateString = Convert.ToString( dateInt );
			DateTime dateValue;
			if ( dateInt < 10000000 )
			{
				return null;
			}

			dateString = Convert.ToString( dateInt / 10000 )
							+ "/" + Convert.ToString( dateInt / 100 % 100 )
							+ "/" + Convert.ToString( dateInt % 100 );

			if ( DateTime.TryParse( dateString, out dateValue ) )
			{
				return dateString;
			}
			else
			{
				return null;
			}
		}
	}
}
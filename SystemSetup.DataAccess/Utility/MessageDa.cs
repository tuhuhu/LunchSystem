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
using System.Data;
//using SystemSetup.DataAccess.Framework;
using SystemSetup.Constants;

namespace SystemSetup.DataAccess
{
	public static partial class UtilityDa
	{
		public static void GetMessage( string messageID,
												string replaceString,
												out string messageClass,
												out string messageContent )
		{
			GetMessage( messageID
						, replaceString
						, String.Empty
						, String.Empty
						, out messageClass
						, out messageContent );
		}

		public static void GetMessage( string messageID,
												string replaceString1,
												string replaceString2,
												out string messageClass,
												out string messageContent )
		{
			GetMessage( messageID
						, replaceString1
						, replaceString2
						, String.Empty
						, out messageClass
						, out messageContent );
		}

		public static void GetMessage( string messageID,
												string replaceString1,
												string replaceString2,
												string replaceString3,
												out string messageClass,
												out string messageContent )
		{
			messageClass = String.Empty;
			messageContent = String.Empty;
            //using ( DBManager dbManager = new DBManager( "stp_GetMessage" ) )
            //{
            //    dbManager.Add( "@MessageID", messageID );
            //    dbManager.Add( "@MessageClass", messageClass, ParameterDirection.Output );
            //    dbManager.Add( "@MessageContent",SqlDbType.NVarChar,200, ParameterDirection.Output );
            //    dbManager.ExecuteNonQuery();
            //    messageClass = dbManager.GetValueInString("@MessageClass");
            //    messageContent = dbManager.GetValueInString("@MessageContent");
            //    if ( String.IsNullOrEmpty(messageContent ) )
            //    {
            //        messageContent = "Message " + messageID + " is not found. ({0},{1},{2})";
            //    }
            //    messageContent = messageContent.Replace("{0}", replaceString1);
            //    messageContent = messageContent.Replace( "{1}", replaceString2 );
            //    messageContent = messageContent.Replace( "{2}", replaceString3 );
            //}
		}
	}
}

//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/03
// Comment		: Create new
//------------------------------------------------------------------------

//using SystemSetup.DataAccess.Framework;
using SystemSetup.Models;
using SystemSetup.UtilityServices;
using System;
using System.Web;

namespace SystemSetup.BusinessServices
{
	public class BaseServices : IDisposable
	{
		private CmnEntityModel cmnEntityModel = null;

		public CmnEntityModel CmnEntityModel
		{
			get
			{
				if ( cmnEntityModel == null )
				{
					if ( HttpContext.Current.Session[ "CmnEntityModel" ] == null )
					{
						HttpContext.Current.Session[ "CmnEntityModel" ] = new CmnEntityModel();
					}
					cmnEntityModel = (CmnEntityModel)HttpContext.Current.Session[ "CmnEntityModel" ];
				}
				return cmnEntityModel;
			}
		}

        //public void Dispose( bool disposing )
        //{
        //    if ( !String.IsNullOrEmpty( this.CmnEntityModel.ErrorMsgCd ) )
        //    {
        //        DBManager.RollbackTransaction();
        //    }
        //    else
        //    {
        //        DBManager.CommitTransaction();
        //    }
        //    DBManager.CloseConnection();
        //}

		public void Dispose()
		{
			//this.Dispose( true );
		}

		/// <summary>
		/// Gets the permision.
		/// </summary>
		/// <returns></returns>
		public string GetPermision()
		{
			return CacheUtil.GetCache<string>("IENTER_SYS_PERMISION");
		}
	}
}
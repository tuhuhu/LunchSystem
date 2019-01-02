//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/17
// Comment		: Create new
//------------------------------------------------------------------------

namespace SystemSetup.UtilityServices.security
{
	using SystemSetup.Constants.Resources;
    using SystemSetup.Models;
	using System.Resources;
	using System.Web;

	/// <summary>
	/// 
	/// </summary>
	public class Permision
	{
		/// <summary>
		/// Gets the CMN entity model.
		/// </summary>
		/// <value>
		/// The CMN entity model.
		/// </value>
		private static CmnEntityModel CmnEntityModel
		{
			get
			{
				if (HttpContext.Current.Session["CmnEntityModel"] == null)
				{
					HttpContext.Current.Session["CmnEntityModel"] = new CmnEntityModel();
				}
				return (CmnEntityModel)HttpContext.Current.Session["CmnEntityModel"];
			}
		}

		/// <summary>
		/// Gets the permision.
		/// </summary>
		/// <returns></returns>
		public static string GetPermision()
		{
			string screenID = CacheUtil.GetCache<string>("IENTER_SYS_PERMISION_SCREEN_ROOT");
			if (string.IsNullOrEmpty(screenID))
			{
				return null;
			}
			ResourceManager managerResource = new ResourceManager(typeof(ScreenIDToCodeValue));
			string menuId = managerResource.GetString(screenID);
			if (string.IsNullOrEmpty(menuId))
			{
				return null;
			}
            //foreach (LoginUserAuthorityMenuModel authorityMenu in CmnEntityModel.UserAuthorityMenu)
            //{
            //    if (menuId.Equals(authorityMenu.AuthorityMenuId))
            //    {
            //        return authorityMenu.Authority;
            //    }
            //}
			return null;
		}

		public static string GetPermisionByCodeValue(string screenCodeValue)
		{
			if (string.IsNullOrEmpty(screenCodeValue))
			{
				return null;
			}
            //foreach (LoginUserAuthorityMenuModel authorityMenu in CmnEntityModel.UserAuthorityMenu)
            //{
            //    if (screenCodeValue.Equals(authorityMenu.AuthorityMenuId))
            //    {
            //        return authorityMenu.Authority;
            //    }
            //}
			return null;
		}
	}
}
//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/03
// Comment		: Create new
//------------------------------------------------------------------------

namespace SystemSetup.Models
{
	public class LoginUserAuthorityMenuModel
	{
		public LoginUserAuthorityMenuModel()
		{
			this.AuthorityMenuId = string.Empty;
			this.Authority = string.Empty;
		}
		public string AuthorityMenuId { get; set; }

		public string Authority { get; set; }
	}
}
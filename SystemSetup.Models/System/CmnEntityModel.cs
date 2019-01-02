//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/03
// Comment		: Create new
//------------------------------------------------------------------------

using SystemSetup.Constants;

namespace SystemSetup.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data;

	/// <summary>
	/// Common entity Model
	/// </summary>
    [Serializable] 
	public class CmnEntityModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CmnEntityModel"/> class.
		/// </summary>
		public CmnEntityModel()
		{
			this.Clear();
			this.CurrentScreenID = string.Empty;
			this.ParentScreenID = string.Empty;
			this.ScreenRoute = string.Empty;
		}

        /// <summary>
        /// Gets or sets company Code
        /// </summary>
        /// <value>
        /// The company Code.
        /// </value>
        public string CompanyCd { get; set; }

        /// <summary>
        /// Gets or sets User account
        /// </summary>
        /// <value>
        /// The User account.
        /// </value>
		public string UserID { get; set; }

		/// <summary>
		/// Gets or sets the role.
		/// </summary>
		/// <value>
		/// The role.
		/// </value>
		public int AuthorityGroupCd { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the name of the menu.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string MenuStatus { get; set; }

        /// <summary>
        /// Gets or sets the seg no of the user.
        /// </summary>
        /// <value>
        /// The seg no of the user.
        /// </value>
        public long UserSegNo { get; set; }

        public int UserType { get; set; }

		/// <summary>
		/// Gets or sets the error MSG cd.
		/// </summary>
		/// <value>
		/// The error MSG cd.
		/// </value>
		public string ErrorMsgCd { get; set; }

		/// <summary>
		/// Gets or sets the error MSG replace string.
		/// </summary>
		/// <value>
		/// The error MSG replace string.
		/// </value>
		public string ErrorMsgReplaceString { get; set; }

		/// <summary>
		/// Gets or sets the user authority menu.
		/// </summary>
		/// <value>
		/// The user authority menu.
		/// </value>
		public List<string> AuthorityList { get; set; }	

		/// <summary>
		/// Gets or sets the current screen identifier.
		/// </summary>
		/// <value>
		/// The current screen identifier.
		/// </value>
		public string CurrentScreenID { get; set; }

		/// <summary>
		/// Gets or sets the parent screen identifier.
		/// </summary>
		/// <value>
		/// The parent screen identifier.
		/// </value>
		public string ParentScreenID { get; set; }

		/// <summary>
		/// Gets or sets the screen route.
		/// </summary>
		/// <value>
		/// The screen route.
		/// </value>
		public string ScreenRoute { get; set; }

        public string RoundingType { get; set; }

        public bool isDisplaySubContractType { get; set; }

        public string formatPathCoverLetterByDocument { get; set; }

        public string formatPathCoverLetterByFax { get; set; }

        public string formatPathCoverLetterByEnvelopeDestination { get; set; }

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{						
			this.UserID = string.Empty;
			this.UserName = string.Empty;			
			this.ErrorMsgCd = string.Empty;
			this.ErrorMsgReplaceString = string.Empty;
            this.AuthorityList = new List<string>();
            this.AuthorityGroupCd = 0;
            this.RoundingType = Constants.RoundingType.RoundDown;
		}
	}
}
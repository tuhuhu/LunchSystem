using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iEnterAsia.iseiQ.Models
{
	/// <summary>
    /// Invalid login user class
    /// </summary>
    [Serializable]
    public class InvalidLoginUser
    {
        public long UserId { get; set; }

        public int InvalidCount { get; set; }

        public InvalidLoginUser(long userId)
        {
            this.UserId = userId;
            InvalidCount = 1;
        }
    }
}
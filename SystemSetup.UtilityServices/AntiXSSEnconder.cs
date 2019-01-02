//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/17
// Comment		: Create new
//------------------------------------------------------------------------

namespace SystemSetup.UtilityServices
{
    using System.IO;
    using System.Web.Util;
    using Microsoft.Security.Application;

    /// <summary>
    /// Summary description for AntiXss
    /// </summary>
    public class AntiXssEncoder : HttpEncoder
    {
        /// <summary>
        /// AntiXssEncoder contructor
        /// </summary>
        public AntiXssEncoder()
        {
        }

        /// <summary>
        /// HtmlEncode encode html
        /// </summary>
        /// <param name="value">source</param>
        /// <param name="output">output</param>
        protected override void HtmlEncode(string value, TextWriter output)
        {
            output.Write(Encoder.HtmlEncode(value));
        }

        /// <summary>
        /// HtmlAttributeEncode
        /// </summary>
        /// <param name="value">source</param>
        /// <param name="output">output</param>
        protected override void HtmlAttributeEncode(string value, TextWriter output)
        {
            output.Write(Encoder.HtmlAttributeEncode(value));
        }

        /// <summary>
        /// HtmlDecode
        /// </summary>
        /// <param name="value">source</param>
        /// <param name="output">output</param>
        protected override void HtmlDecode(string value, TextWriter output)
        {
            base.HtmlDecode(value, output);
        }
    }
}
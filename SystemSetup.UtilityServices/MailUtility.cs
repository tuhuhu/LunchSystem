//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/17
// Comment		: Create new
//------------------------------------------------------------------------

//using SystemSetup.DataAccess.Common;
using SystemSetup.Models;
using SystemSetup.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Threading;

namespace SystemSetup.UtilityServices
{
	/// <summary>
	///
	/// </summary>
	public class MailUtility
	{
        public int SentMail(string email, string password)
        {
            int success = 0;
            try
            {
                var msg = new MailMessage();

                msg.From = new MailAddress(ConfigurationManager.AppSettings[ConfigurationKeys.SMTP_USER]);
                msg.To.Add(email);
                msg.Subject = SystemSetup.Constants.Resources.Mail.Subject;
                msg.Body = String.Format(SystemSetup.Constants.Resources.Mail.MailBody, password, ConfigurationManager.AppSettings[ConfigurationKeys.SMTP_SUPPORT]);
                using (var client = new SmtpClient())
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings[ConfigurationKeys.SMTP_USER], ConfigurationManager.AppSettings[ConfigurationKeys.SMTP_PASS]);
                    client.Host = ConfigurationManager.AppSettings[ConfigurationKeys.SMTP_SERVER];
                    client.Port = Convert.ToInt32(ConfigurationManager.AppSettings[ConfigurationKeys.SMTP_PORT]);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    try
                    {
                        client.Send(msg);
                        msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                        success = 1;
                        client.Dispose();
                    }
                    catch (SmtpFailedRecipientException ex)
                    {
                        success = 0;
                        Console.Write("Error" + ex.Message);
                    }
                    finally
                    {
                        client.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                success = 0;
                Console.Write("Error" + ex.Message);
            }
            return success;
        }

        public static int SendMail(string mailTo, string CC, string BCC, string subject, string body)
        {
            int success = 0;
            try
            {
                var msg = new MailMessage();
                msg.From = new MailAddress(ConfigurationManager.AppSettings[ConfigurationKeys.SMTP_USER]);
                if (!string.IsNullOrEmpty(mailTo))
                {
                    foreach (var mail in mailTo.Split(';'))
                    {
                        msg.To.Add(mail);
                    }
                }

                if (!string.IsNullOrEmpty(CC))
                {
                    foreach (var mail in CC.Split(';'))
                    {
                        msg.CC.Add(mail);
                    }
                }

                if (!string.IsNullOrEmpty(BCC))
                {
                    foreach (var mail in BCC.Split(';'))
                    {
                        msg.Bcc.Add(mail);
                    }
                }

                msg.Subject = subject ?? "";
                msg.Body = body ?? "";

                var client = new SmtpClient();
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings[ConfigurationKeys.SMTP_USER], ConfigurationManager.AppSettings[ConfigurationKeys.SMTP_PASS]);
                client.Host = ConfigurationManager.AppSettings[ConfigurationKeys.SMTP_SERVER];
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings[ConfigurationKeys.SMTP_PORT]);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                msg.DeliveryNotificationOptions = DeliveryNotificationOptions.None;

                Thread threadSendMail = new Thread(delegate()
                {
                    client.Send(msg);
                });

                threadSendMail.Start();
                success = 1;
            }
            catch (Exception ex)
            {
                success = 0;
                //logger.Error(ex);
            }
            return success;
        }
	}
}
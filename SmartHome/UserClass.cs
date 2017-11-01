using EASendMailRT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{

    class UserClass
    {
        public string Email { get; set; }
        public string EncryptedPassword { get; set; }
        private string password;

        public string Hash { get; set; }
        public AccesLevel Level { get; set; } = AccesLevel.Minimal;

        public UserClass(User user)
        {
            Email = user.email;
            EncryptedPassword = user.password;
            Hash = user.hash;
            Level = user.accesLevel;
        }

        public UserClass()
        {

        }
        public UserClass(Admin user)
        {
            Email = user.email;
            EncryptedPassword = user.password;
            Hash = user.hash;
            Level = AccesLevel.Admin;
        }

        public void SaveUser()
        {
            DBInstance.SaveUser(this);
        }

        

        public static UserClass GetUser(string pass, string email)
        {
           var admin = DBInstance.FindAdmin(email, pass);
            if (admin != null)
            {
                return new UserClass(admin);
            }               
            else
            {
                var user = DBInstance.FindUser(email, pass);
                if (user == null) return null;
                return new UserClass(user);
            }           
        }

        public static UserClass GetUser(string email)
        {
            var user = DBInstance.FindUser(email);
            if (user == null) return null;
            return new UserClass(user);

        }

        public void ModifyPassword(string newPassword)
        {
            var user = new UserClass(DBInstance.FindUser(this.Email));
            if (user != null)
            {
                string newEncryptedPassword = UserManager.EncodePassword(newPassword, Hash);
                user.EncryptedPassword = newEncryptedPassword;
            }
            DBInstance.UpdateUser(user);
        }

        public async Task<bool> SendMail(string body, string title)
        {
            String Result = "";
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");
                SmtpClient oSmtp = new SmtpClient();

                // Set your gmail email address
                oMail.From = new MailAddress("okosotthon.help@gmail.com");

                // Add recipient email address, please change it to yours
                oMail.To.Add(new MailAddress(Email));

                // Set email subject and email body text
                oMail.Subject = title;
                oMail.TextBody = body;

                // Gmail SMTP server
                SmtpServer oServer = new SmtpServer("smtp.gmail.com");

                // User and password for ESMTP authentication
                oServer.User = "okosotthon.help@gmail.com";
                oServer.Password = "Alma1234";

                // Use 465 port
                oServer.Port = 465;
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                await oSmtp.SendMailAsync(oServer, oMail);
                Result = "Email was sent successfully!";
                return true;
            }
            catch (Exception ep)
            {
                Result = String.Format("Failed to send email with the following error: {0}", ep.Message);
                return false;
            }

            // Display Result by Diaglog box
            Windows.UI.Popups.MessageDialog dlg = new
                Windows.UI.Popups.MessageDialog(Result);

            await dlg.ShowAsync();

        }
    }
}

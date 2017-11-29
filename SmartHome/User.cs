using EASendMailRT;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    [Table("User")]
    class User
    {
        [Key]
        public string Email { get; set; }
        [Required]
        public string EncryptedPassword { get; set; }
        private string password;
        [Required]
        public string Salt { get; set; }
        [Required]
        public AccesLevel Level { get; set; } = AccesLevel.Minimal;

        public User()
        {
        }

        public void SaveUser()
        {
            DBInstance.SaveUser(this);
        }

        public void ModifyPassword(string newPassword)
        {
            var user = DBInstance.FindUser(this.Email);
            if (user != null)
            {
                user.GetSalt();
                string newEncryptedPassword = user.EncodePassword(newPassword);
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

        public void GetSalt()
        {
            var rng = RandomNumberGenerator.Create();
            var buff = new byte[32];
            rng.GetBytes(buff);
            Salt = Convert.ToBase64String(buff);

        }

        public string EncodePassword(string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] src = Encoding.Unicode.GetBytes(Salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = SHA256.Create();
            byte[] inarray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inarray);
        }
    }
}

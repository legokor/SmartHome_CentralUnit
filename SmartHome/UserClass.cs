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
        public AccesLevel Level { get; set; } = AccesLevel.Zero;

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
            Level = AccesLevel.Full;
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
    }
}

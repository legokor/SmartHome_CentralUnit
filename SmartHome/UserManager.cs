using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SmartHome
{
    enum AccesLevel { NotLogined ,Minimal, Normal, Admin };
    abstract class UserManager
    {
        public static bool IsLogined = false;
        public static UserClass LoginedUser;
        public static AccesLevel Level = AccesLevel.NotLogined;

        public static bool Login(string pass, string email)
        {
            var user = UserManager.GetUser(email);
            if (user != null && user.EncodePassword(pass) == user.EncryptedPassword)
            {
                IsLogined = true;
                LoginedUser = user;
                Level = user.Level;
                return true;
            }
            else
            {
                IsLogined = false;
                LoginedUser = null;
                return false;
            }
        }

        public static void Logout()
        {
            IsLogined = false;
            LoginedUser = null;
            Level = AccesLevel.NotLogined;
        }

        public static List<UserClass> GetAllUser()
        {
            var users = DBInstance.AvailableUser();
            List<UserClass> userclasses = new List<UserClass>();
            foreach (var user in users)
            {
                if (user.email != LoginedUser.Email)
                {
                    userclasses.Add(new UserClass(user));
                }
            }
            return userclasses;
        }

        public static UserClass GetUser(string email)
        {
            var user = DBInstance.FindUser(email);
            if (user == null) return null;
            return new UserClass(user);

        }

        public static void CreateUser(string pass, string email)
        {
            var user = new UserClass();
            user.Email = email;
            user.Level = AccesLevel.Minimal;
            user.GetSalt();
            user.EncryptedPassword = user.EncodePassword(pass);
            DBInstance.SaveUser(user);
            user.SendMail("You have registered with your "+email+" email address!", "Succesfully registration in the SmartHome system!");
        }



      

        public static void SetAccesLevel(string email, AccesLevel newLevel)
        {
            if (Level != AccesLevel.Admin) return;
            var user = new UserClass(DBInstance.FindUser(email));
            if (user != null)
            {
                user.Level = newLevel;
            }
            DBInstance.UpdateUser(user);
        }

        public static async Task<bool> SendForgottenEmail(string email)
        {
            Random random = new Random();
            string password = null;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            password = new string(Enumerable.Repeat(chars, 8)
                 .Select(s => s[random.Next(s.Length)]).ToArray());
            var user = UserManager.GetUser(email);
            if (user == null) return false;

            if (await user.SendMail("Your temporary password is: " + password, "Forgotten password"))
            {
                user.ModifyPassword(password);
                return true;
            }
            else return false;
            
        }   
        
    }
}

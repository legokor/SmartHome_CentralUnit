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
        public static User LoginedUser;
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

        public static List<User> GetAllUser()
        {
            var users = DBInstance.AvailableUser();
            List<User> users2 = new List<User>();
            foreach (var user in users)
            {
                if (user.Email != LoginedUser.Email)
                {
                    users2.Add(user);
                }
            }
            return users2;
        }

        public static User GetUser(string email)
        {
            var user = DBInstance.FindUser(email);
            if (user == null) return null;
            return user;

        }

        public static void CreateUser(string pass, string email)
        {
            var user = new User();
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
            var user = DBInstance.FindUser(email);
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

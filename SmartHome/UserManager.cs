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
            var user = UserClass.GetUser(pass, email);
            if (user != null)
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

        public static void CreateUser(string pass, string email)
        {
            var user = new UserClass();
            user.Email = email;
            user.Level = AccesLevel.Minimal;
            user.Hash = GetSalt();
            user.EncryptedPassword = EncodePassword(pass, user.Hash);
            DBInstance.SaveUser(user);
            user.SendMail("You have registered with your "+email+" email address!", "Succesfully registration in the SmartHome system!");
        }

        public static string GetSalt()
        {
            var rng = RandomNumberGenerator.Create();
            var buff = new byte[32];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);

        }

        public static string EncodePassword(string password, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] src = Encoding.Unicode.GetBytes(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = SHA256.Create();
            byte[] inarray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inarray);
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
            var user = UserClass.GetUser(email);
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

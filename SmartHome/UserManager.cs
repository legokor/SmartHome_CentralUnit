using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SmartHome
{
    enum AccesLevel { Zero, Normal, Full };
    abstract class UserManager
    {
        public static bool IsLogined = false;
        public static UserClass LoginedUser;
        public static AccesLevel Level;

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
        }

        public static void CreateUser(string pass, string email)
        {
            var user = new UserClass();
            user.Email = email;
            user.Level = AccesLevel.Zero;
            user.Hash = GetSalt();
            user.EncryptedPassword = EncodePassword(pass, user.Hash);
            DBInstance.SaveUser(user);
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
            if (Level != AccesLevel.Full) return;
            var user = new UserClass(DBInstance.FindUser(email));
            if (user != null)
            {
                user.Level = newLevel;
            }


        }
    }
}

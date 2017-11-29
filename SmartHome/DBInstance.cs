using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    abstract class  DBInstance
    {
        public static void SaveData(DataSample data)
        {
            using (var context = new Model())
            {               
                UdpServer.UploadToServer(data);
                context.DataSamples.Add(data);
                context.SaveChanges();
            }
        }

        public static void SaveUnit(Unit unit)
        {
            using (var context = new Model())
            {             
                context.Units.Add(unit);
                context.SaveChanges();
            }
        }
        public static List<Unit> LoadUnits()
        {
            using (var context = new Model())
            {
                var units = context.Units .ToList();

                return units;
            }
        }


        public static List<DataSample> LoadData(string id)
        {
            using (var context = new Model())
            {
                var samples = context.DataSamples
                    .Where(b => b.SenderId.ToString() == (id))
                    .ToList();

                return samples;
            }
        }

        public static void SaveUser(User user)
        {
            using (var context = new Model())
            {             
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public static User FindUser(string email)
        {
            using (var context = new Model())
            {
                var user = context.Users
                    .Where(b => b.Email == email)
                    .ToList();
                if (!user.Any()) return null;
                return user.First();
            }
        }

        public static List<User> AvailableUser()
        {
            using (var context = new Model())
            {
                var user = context.Users.ToList();
                return user;
            }
        }

        public static void SaveAdmin(User user)
        {
            using (var context = new Model())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }


        public static void UpdateUser(User user)
        {
            using (var context = new Model())
            {
               foreach(var newUser in context.Users.Where(s=>s.Email == user.Email))
                {
                    newUser.Email = user.Email;
                    newUser.EncryptedPassword = user.EncryptedPassword;
                    newUser.Level = user.Level;
                    newUser.Salt = user.Salt;
                }
                context.SaveChanges();
            }
        }

        public static void RemoveUser(User user)
        {
            using (var context = new Model())
            {
                foreach (var userToDelete in context.Users.Where(s => s.Email == user.Email))
                {
                    context.Users.Remove(userToDelete);
                }
                context.SaveChanges();
            }
        }

    }
}

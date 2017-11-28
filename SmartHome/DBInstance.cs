using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    abstract class  DBInstance
    {
        public static void SaveData(DataElement data)
        {
            using (var context = new Model())
            {
                var sample = new DataSample
                {
                    SenderId = Int32.Parse(data.Id),
                    Movement = Int32.Parse(data.Movement),
                    Temperature = double.Parse(data.Temperature),
                    Humidity = double.Parse(data.Humidity),
                    CoLevel = double.Parse(data.Co),
                    SmokeLevel = double.Parse(data.Smoke),
                    LpgLevel = double.Parse(data.Lpg)
                 };
                UdpServer.UploadToServer(sample);
                context.DataSamples.Add(sample);
                context.SaveChanges();
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

        public static void SaveUser(UserClass user)
        {
            using (var context = new Model())
            {
                var newUser = new User
                {
                    email = user.Email,
                    password = user.EncryptedPassword,
                    accesLevel = user.Level,
                    salt = user.Salt
                    
                };
                context.Users.Add(newUser);
                context.SaveChanges();
            }
        }

        public static User FindUser(string email)
        {
            using (var context = new Model())
            {
                var user = context.Users
                    .Where(b => b.email == email)
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

        public static void SaveAdmin(UserClass user)
        {
            using (var context = new Model())
            {
                var newUser = new User
                {
                    email = user.Email,
                    password = user.EncryptedPassword,
                    accesLevel = AccesLevel.Admin,
                    salt = user.Salt

                };
                context.Users.Add(newUser);
                context.SaveChanges();
            }
        }


        public static void UpdateUser(UserClass user)
        {
            using (var context = new Model())
            {
               foreach(var newUser in context.Users.Where(s=>s.email == user.Email))
                {
                    newUser.email = user.Email;
                    newUser.password = user.EncryptedPassword;
                    newUser.accesLevel = user.Level;
                    newUser.salt = user.Salt;
                }
                context.SaveChanges();
            }
        }

        public static void RemoveUser(UserClass user)
        {
            using (var context = new Model())
            {
                foreach (var userToDelete in context.Users.Where(s => s.email == user.Email))
                {
                    context.Users.Remove(userToDelete);
                }
                context.SaveChanges();
            }
        }

    }
}

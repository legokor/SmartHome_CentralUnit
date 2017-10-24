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
                var blog = new Sample
                {
                    senderId = Int32.Parse(data.Id),
                    movement = Int32.Parse(data.Movement)
                };
                context.Samples.Add(blog);
                context.SaveChanges();
            }
        }

        public static List<Sample> LoadData(string id)
        {
            using (var context = new Model())
            {
                var blogs = context.Samples
                    .Where(b => b.senderId.ToString() == (id))
                    .ToList();

                return blogs;
            }
        }

    }
}

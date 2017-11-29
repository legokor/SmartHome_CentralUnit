using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{

    [Table("Unit")]
     class Unit
    {
        [Key]
        public string BuiltinID { get; set; }
        public string Id { get; set; }
        public string Location { get; set; } = "";
        public string Type { get; set; } = "";
        public bool Auto { get; set; } = true;
    }
}

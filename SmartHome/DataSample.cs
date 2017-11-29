using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    [Table("DataSample")]
    class DataSample
    {
        [Key]
        public Guid SamplingId { get; set; }
        [Required]
        public string SenderId { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double CoLevel { get; set; }
        public double SmokeLevel { get; set; }
        public double LpgLevel { get; set; }
        public DateTime TimeStamp { get; set; }

        public int Movement { get; set; }
    }
}

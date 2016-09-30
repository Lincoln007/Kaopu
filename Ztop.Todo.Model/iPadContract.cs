using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("ipad_Contracts")]
    public class iPadContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string PartA { get; set; }
        public string PartB { get; set; }
        public string Place { get; set; }
        public DateTime? Time { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string File { get; set; }
        public double Money { get; set; }
        public string PayWay { get; set; }
        [NotMapped]
        public List<Register_iPad> Contract_iPads { get; set; }
        [NotMapped]
        public List<iPad> iPads { get; set; }
    }
}

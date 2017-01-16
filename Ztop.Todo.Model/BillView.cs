using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("bills_view")]
    public class BillView
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime? Time { get; set; }
        public double Money { get; set; }
        public string Account { get; set; }
        public string Summary { get; set; }
        public string Remark { get; set; }
        public double Balance { get; set; }
        public double Leave { get; set; }
        public Association Association { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        [Column(TypeName ="int")]
        public Company Company { get; set; }
        [Column(TypeName = "int")]
        public Cost? Cost { get; set; }
        public string TName { get; set; }
        //[Column(TypeName = "int")]
        //public Category? Category { get; set; }
        [NotMapped]
        public List<BillContract> Bill_Contracts { get; set; }
    }
}

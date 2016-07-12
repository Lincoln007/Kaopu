using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ztop.Todo.Model
{
    [Table("billcontract")]
    public class BillContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ContractID { get; set; }
        public int BillID { get; set; }
        public double Price { get; set; }
        [NotMapped]
        public Contract Contract { get; set; }
        [NotMapped]
        public Bill Bill { get; set; }
    }
}

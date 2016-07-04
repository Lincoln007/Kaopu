using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ztop.Todo.Model
{
    [Table("invoicebillaccount")]
    public class InvoiceBillAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public double Price { get; set; }
        public int IID { get; set; }
        public int BID { get; set; }
        [NotMapped]
        public Bill Bill { get; set; }
    }
}

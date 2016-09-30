using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("ipad_invoices")]
    public class iPadInvoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public string Number { get; set; }
        public double Money { get; set; }
        public string File { get; set; }
        [NotMapped]
        public int Count
        {
            get
            {
                if (iPads == null)
                {
                    return 0;
                }
                else
                {
                    return iPads.Count;
                }
            }
        }
        public string Buyer { get; set; }
        public string Enter { get; set; }
        [NotMapped]
        public List<Register_iPad> Relations { get; set; }
        [NotMapped]
        public List<iPad> iPads
        {
            get
            {
                if (Relations == null)
                {
                    return null;
                }
                return Relations.Select(e => e.iPad).ToList();
            }
        }
    }
}

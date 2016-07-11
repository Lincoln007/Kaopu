using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ztop.Todo.Model
{
    [Table("article")]
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string OtherSide { get; set; }
        public double Money { get; set; }
        [MaxLength(1023)]
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public List<Contract> Contracts { get; set; } 
    }
}

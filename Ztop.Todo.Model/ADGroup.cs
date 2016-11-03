using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("ad_groups")]
    public class ADGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Column(TypeName ="int")]
        public ADType Type { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string Code
        {
            get
            {
                try
                {
                    return Name.Substring(0, 4);
                }
                catch
                {
                    return null;
                }
            }
        }
        public int OID { get; set; }
        [NotMapped]
        public ADGroup Parent { get; set; }
    }

    public enum ADType
    {
        Organication,
        Group,
        Catalog,
        Other
    }
}

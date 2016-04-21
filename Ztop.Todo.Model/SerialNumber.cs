using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("serialnumbers")]
    public class SerialNumber
    {
        public SerialNumber()
        {
            Number = string.Format("{0}{1}", DateTime.Now.Year.ToString("0000"), DateTime.Now.Month.ToString("00"));
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Number { get; set; }
        public int NumberExt { get; set; }
        [NotMapped]
        /// <summary>
        /// 单据编号
        /// </summary>
        public string Coding {
            get
            {
                return Number + NumberExt.ToString("0000");
            }
        }
        /// <summary>
        /// 报销单  唯一编号
        /// </summary>
        public int SID { get; set; }
    }
}

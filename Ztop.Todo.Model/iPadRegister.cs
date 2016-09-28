using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("ipad_Registers")]
    public class iPadRegister
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        [NotMapped]
        public string Number
        {
            get
            {
                return string.Format("{0}{1}{2}{3}", Time.Year, Time.Month.ToString("00"), Time.Day.ToString("00"), ID.ToString("00000000"));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 用途/说明
        /// </summary>
        public string Mark { get; set; }
        public string Borrower { get; set; }
        public DateTime BorrowTime { get; set; }
        public string Enter { get; set; }
        public bool Revert { get; set; }
        [NotMapped]
        public List<iPad> iPads { get; set; }

    }
}

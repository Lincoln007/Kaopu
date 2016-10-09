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
        /// <summary>
        /// 借用者
        /// </summary>
        public string Borrower { get; set; }
        /// <summary>
        /// 借用时间
        /// </summary>
        public DateTime BorrowTime { get; set; }
        /// <summary>
        /// 信息录入人
        /// </summary>
        public string Enter { get; set; }
        [NotMapped]
        public bool Revert
        {
            get
            {
                if (Register_iPads == null)
                {
                    return false;
                }
                return Register_iPads.Where(e => e.Revert == iPadRevert.Using).Count() == 0;
            }
        }
        [NotMapped]
        public List<Register_iPad> Register_iPads { get; set; }
        [NotMapped]
        public List<iPad> iPads
        {
            get
            {
                if (Register_iPads == null)
                {
                    return null;
                }
                return Register_iPads.Select(e => e.iPad).ToList();
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ztop.Todo.Model
{
    [Table("contractfiles")]
    public class ContractFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ContractID { get; set; }
        [MaxLength(1023)]
        public string SavePath { get; set; }
        [MaxLength(255)]
        public string FileName { get; set; }
        public int FileSize { get; set; }
        /// <summary>
        /// 是否移除
        /// </summary>
        public bool Remove { get; set; }
    }


    public enum TodoFile
    {
        Contract,
        iPad_Contract
    }

}

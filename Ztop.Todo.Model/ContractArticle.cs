using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ztop.Todo.Model
{
    /// <summary>
    /// 合同对应的多个项目 关系表
    /// </summary>
    [Table("contract_article")]
    public class ContractArticle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ContractID { get; set; }
        public int ArticleID { get; set; }
    }
}

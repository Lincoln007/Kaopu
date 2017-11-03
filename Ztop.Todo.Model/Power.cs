 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("Power")]
    public class Power
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
        public string Glyphicon { get; set; }
        public string Class { get; set; }
        public PowerEnum PowerEnum { get; set; }
        public PowerType PowerType { get; set; }
        public int PowerId { get; set; }
        public int OASystemId { get; set; }
        [ForeignKey("OASystemId")]
        public virtual OASystem System { get; set; }
        public virtual List<PowerItem> Items { get; set; }
        public string Remark { get; set; }

    }

    [Table("Power_Items")]
    public class PowerItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int PowerId { get; set; }
        [ForeignKey("PowerId")]
        public virtual Power Power { get; set; }
    }

    [Table("OASystem")]
    public class OASystem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int Order { get; set; }
        public OASystemClass OASystemClass { get; set; }
        public bool Deleted { get; set; }
        public virtual List<Power> Powers { get; set; }
        
    }

    


    public class PowerBase
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
        public string Glyphicon { get; set; }
       public string SystemName { get; set; }
        public string SystemClass { get; set; }
    }

    public enum PowerEnum
    {
        [Description("按钮")]
        Button,
        [Description("链接")]
        Link,
    }
    public enum PowerType
    {
        [Description("弹窗")]
        Windows,
        [Description("地址")]
        Address
    }
}

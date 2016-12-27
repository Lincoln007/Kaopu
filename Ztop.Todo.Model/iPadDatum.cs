using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("ipad_datums")]
    public class iPadDatum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Address { get; set; }
        public string Path { get; set; }
        [NotMapped]
        public string DownLoadPath
        {
            get
            {
                return string.Format("ftp://{0}:{1}@{2}", "ftpUser", "Ztop123456", Path);
            }
        }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public bool Repeal { get; set; }
    }
}

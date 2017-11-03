using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("Project_view")]
    public class ProjectView
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public int Year { get; set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public int SerialNumber { get; set; }
        public string Number { get; set; }
        public string CityName { get; set; }
        public string Town { get; set; }
        public string Mark { get; set; }
        public string Name { get; set; }
        public int ProjectTypeId { get; set; }
        public int FID { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public bool Deleted { get; set; }
        public string TypeName { get; set; }
        public string TypeChars { get; set; }
        [NotMapped]
        public string Serial
        {
            get { return string.Format("{0}{1}", Year, SerialNumber.ToString("0000")); }
        }
    }
}

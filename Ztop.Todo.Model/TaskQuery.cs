using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Common;

namespace Ztop.Todo.Model
{
    [Table("task_query")]
    public class TaskQuery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int UserID { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        private TaskQueryParameter _parameter;

        public TaskQueryParameter ConvertToTaskQueryParameter()
        {
            if (_parameter == null && !string.IsNullOrEmpty(Content))
            {
                _parameter = JsonExtension.ToObject<TaskQueryParameter>(Content);
            }
            return _parameter;
        }
    }
}

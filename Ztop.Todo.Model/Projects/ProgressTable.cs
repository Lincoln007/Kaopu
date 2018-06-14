using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("progress_table")]
    public class ProgressTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public int Year { get; set; }
        public double Percent { get; set; }
        public string Content { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
        public virtual List<WorkLoad> WorkLoads { get; set; }

        public static List<ProgressTable> Generate(int projectId,int[] users, int[] year, double[] percent, string[] content,Queue<int> userIds,double[] ppercent,string[] pContent)
        {
            var list = new List<ProgressTable>();
            var l = userIds.Dequeue();
            for(var i = 0; i < year.Length; i++)
            {
                var entry = new ProgressTable
                {
                    Year = year[i],
                    Percent = percent[i],
                    Content = content[i],
                    ProjectId=projectId,
                    WorkLoads=new List<WorkLoad>()
                };
                for(var j = 0; j < users.Length; j++)
                {
                    if (l == users[j])
                    {
                        entry.WorkLoads.Add(new WorkLoad { UserId = users[j], Percent = ppercent[i * users.Length + j],Content=pContent[i*users.Length+j] });
                        if (userIds.Count == 0)
                        {
                            break;
                        }
                        else
                        {
                            l = userIds.Dequeue();
                        }
                       
                    }
                }

                list.Add(entry);
               
            }
            return list;
        }
    }
}

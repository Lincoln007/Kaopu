using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Ztop.Todo.Manager;
using Ztop.Todo.Model;

namespace Ztop.Todo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new DataBookManager();
            int times = 500;
            string name = DateTime.Today.ToLongDateString();
            string textName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name+".txt");
            var sw = new StreamWriter(textName);
            System.Console.SetOut(sw);
            while (true)
            {
                var list = manager.GetPastDue();
                string error = "";
                if (DateTime.Today.ToLongDateString() != name)
                {
                    sw.Flush();
                    sw.Close();
                    name = DateTime.Today.ToLongDateString();
                    textName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name + ".txt");
                    sw = new StreamWriter(textName);
                    System.Console.SetOut(sw);
                }
                foreach (var item in list)
                {
                    System.Console.WriteLine(string.Format("开始操作将{0} 从{1}中移除", item.Name, item.GroupName));
                    error = string.Empty;
                    var record = new Record
                    {
                        DID = item.ID,
                        Flag = manager.Examine(item, out error),
                        Mark = error
                    };
                    manager.Records(record);
                    if (record.Flag)
                    {
                        System.Console.WriteLine(string.Format("成功将{0}从{1}中移除", item.Name, item.GroupName));
                    }
                    else
                    {
                        System.Console.WriteLine(string.Format("将{0}从{1}中移除，失败！！！", item.Name, item.GroupName));
                    }
                }
                if (list.Count == 0)
                {
                    times = 100000;
                }
                Thread.Sleep(times);
                
            }
            

        }
    }
}

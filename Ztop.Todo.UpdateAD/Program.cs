using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Ztop.Todo.ActiveDirectory;
using Ztop.Todo.Manager;
using Ztop.Todo.Model;

namespace Ztop.Todo.UpdateAD
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Work();
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }

           
        }

        static void Work()
        {
            var manager = new AD_GroupManager();
            var dict = ADController.GetGroupDict();
            var oldgranication = manager.GetOrganication();
            foreach (var entry in dict)
            {
                var name = entry.Key;
                var current = oldgranication.FirstOrDefault(e => e.Name.ToUpper() == name.ToUpper());
                if (current == null)
                {
                    current = manager.Save(new Model.ADGroup { Name = name });
                    var insertgroup = entry.Value.Select(e => new ADGroup {  Name = e.Name, Type = ADType.Group, OID = current.ID }).ToList();
                    manager.Save(insertgroup);
                }
                else
                {
                    var oldgroup = manager.GetByOID(current.ID);
                    oldgranication.Remove(current);
                    foreach (var item in entry.Value)
                    {
                        //var gcode = item.Name.Substring(0, 6);
                        var gname = item.Name;
                        var gcurrent = oldgroup.FirstOrDefault(e => e.Name.ToUpper() == gname.ToUpper());
                        if (gcurrent == null)
                        {
                            gcurrent = manager.Save(new ADGroup { Name = gname, OID = current.ID });
                        }
                        else
                        {
                            oldgroup.Remove(gcurrent);
                            if (gcurrent.OID != current.ID)
                            {
                                gcurrent.OID = current.ID;
                                manager.Update(gcurrent);
                            }
                        }
                    }
                    if (oldgroup.Count > 0)
                    {
                        manager.Remove(oldgroup);
                    }
                }


            }

            if (oldgranication.Count > 0)
            {
                manager.Remove(oldgranication);
            }
        }
    }
}

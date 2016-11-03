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
        private static AD_GroupManager _manager { get; set; }
        /// <summary>
        /// 除了Group 类型以外的记录
        /// </summary>
        private static List<ADGroup> _parents { get; set; }
        static void Main(string[] args)
        {
            try
            {
                _manager = new AD_GroupManager();
                _parents = _manager.Get().Where(e => e.Type != ADType.Group).ToList();
                Work();
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }

           
        }

        private static void CheckRecord(string name,ADType type,List<ActiveDirectory.Group> values)
        {
            var current = _parents.Where(e => e.Name.ToUpper() == name.ToUpper() && e.Type == type).FirstOrDefault();
            if (current == null)
            {
                current = _manager.Save(new ADGroup { Name = name, Type = type });
                var insertgroup = values.Select(e => new ADGroup { Name = e.Name, Type = ADType.Group, OID = current.ID }).ToList();
                _manager.Save(insertgroup);
            }
            else
            {
                var oldgroup = _manager.GetByOID(current.ID);
                _parents.Remove(current);
                foreach(var item in values)
                {
                    var gcurrent = oldgroup.FirstOrDefault(e => e.Name.ToUpper() == item.Name.ToUpper());
                    if (gcurrent == null)
                    {
                        _manager.Save(new ADGroup { Name = item.Name, Type = ADType.Group, OID = current.ID });
                    }
                    else
                    {
                        oldgroup.Remove(gcurrent);
                    }
                }

                if (oldgroup.Count > 0)
                {
                    _manager.Remove(oldgroup);
                }
            }
        }


        static void Work()
        {
            var big = XmlHelper.GetDitrict();
            var codes = big.Select(e => e.Substring(0, 4)).ToList();
            var dict = ADController.GetGroupDict();
            foreach (var entry in dict)
            {
                var name = entry.Key;
                ADType type = ADType.Other;
                if (name == "目录权限")//目录权限
                {
                    #region 更新目录权限组
                    type = ADType.Catalog;
                    #endregion
                }
                else
                {
                    if (name.Length >= 4)
                    {
                        var key = name.Substring(0, 4);
                        if (codes.Contains(key))//浙江省行政区权限组
                        {
                            type = ADType.Organication;

                            #region 更新城市权限组
                            //var current = oldgranication.FirstOrDefault(e => e.Name.ToUpper() == name.ToUpper());
                            //if (current == null)
                            //{
                            //    current = manager.Save(new Model.ADGroup { Name = name });
                            //    var insertgroup = entry.Value.Select(e => new ADGroup { Name = e.Name, Type = ADType.Group, OID = current.ID }).ToList();
                            //    manager.Save(insertgroup);
                            //}
                            //else
                            //{
                            //    var oldgroup = manager.GetByOID(current.ID);
                            //    oldgranication.Remove(current);
                            //    foreach (var item in entry.Value)
                            //    {
                            //        var gname = item.Name;
                            //        var gcurrent = oldgroup.FirstOrDefault(e => e.Name.ToUpper() == gname.ToUpper());
                            //        if (gcurrent == null)
                            //        {
                            //            gcurrent = manager.Save(new ADGroup { Name = gname, OID = current.ID });
                            //        }
                            //        else
                            //        {
                            //            oldgroup.Remove(gcurrent);
                            //            if (gcurrent.OID != current.ID)
                            //            {
                            //                gcurrent.OID = current.ID;
                            //                manager.Update(gcurrent);
                            //            }
                            //        }
                            //    }
                            //    if (oldgroup.Count > 0)
                            //    {
                            //        manager.Remove(oldgroup);
                            //    }
                            //}
                            #endregion
                        }
                    }
                }
                CheckRecord(name, type, entry.Value);
            }

            if (_parents.Count > 0)
            {
                _manager.Remove(_parents);
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.ActiveDirectory
{
    public class Group
    {
        public Group()
        {
            Children = new List<Group>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public string Descriptions { get; set; }
        public string Ou { get; set; }
        public List<Group> Children { get; set; }

        public static string TreeToString(Group Group)
        {
            var value = string.Empty;
            if (Group == null || string.IsNullOrEmpty(Group.Name))
            {
                return null;
            }
            value += "{ label:'" + Group.Name + "',children:[";
            foreach (Group item in Group.Children)
            {
                value += TreeToString(item);
            }
            value += "]},";
            return value;
        }
    }

    public class TreeObject
    {
        public TreeObject()
        {
            children = new List<TreeObject>();
        }
        public string label { get; set; }
        public List<TreeObject> children { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Common
{
    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null) return null;
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static DescriptionAttribute[] GetDescriptAttr(this FieldInfo field)
        {
            if (field != null)
            {
                return (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }
            return null;
        }
        public static T GetEnum<T>(string description)
        {
            Type _type = typeof(T);
            foreach(FieldInfo field in _type.GetFields())
            {
                DescriptionAttribute[] _curDesc = field.GetDescriptAttr();
                if (_curDesc != null && _curDesc.Length > 0)
                {
                    if (_curDesc[0].Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }
            throw new ArgumentException(string.Format("{0}未能找到对应的枚举.", description), "description");
        }
    }
}

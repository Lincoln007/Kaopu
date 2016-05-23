using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Ztop.Todo.Update
{
    public class IniClass
    {
        public string IniPath { get; set; }
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string kek, string def, StringBuilder retVal, int size, string filePath);
        public IniClass(string iniPath)
        {
            this.IniPath = iniPath;
        }
        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">项目名称（如【TypeName】）</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void IniWriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this.IniPath);
        }
        /// <summary>
        /// 读出INI文件
        /// </summary>
        /// <param name="section">项目名称（如【TypeName】）</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string IniReadValue(string section,string key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(section, key, "", temp, 500, this.IniPath);
            return temp.ToString();
        }
        public bool ExistIniFile()
        {
            return File.Exists(IniPath);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Common;

namespace Ztop.Todo.WindowsClient
{
    public class TODOFTP
    {
        private Dictionary<string,string> _dict { get; set; }
        private string _error { get; set; }
        private MainForm _mainForm { get; set; }
        public TODOFTP(MainForm mainForm,Dictionary<string,string> dict)
        {
            _dict = dict;
            _mainForm = mainForm;
        }

        public void Start()
        {
            if (_dict != null)
            {
                string result = string.Empty;
                string path = string.Empty;
                foreach(var entry in _dict)
                {
                    path = entry.Key.Replace(@"\", "\\");
                    if (System.IO.File.Exists(path))
                    {
                        try
                        {
                            FTPHelper.UploadFile(path, entry.Value);
                        }catch(Exception ex)
                        {
                            _error += string.Format("上传文件失败：{0}，错误原因：{1}**", path, ex.ToString());
                            continue;
                        }
                    }
                    else
                    {
                        _error += string.Format("文件路径未识别，路径：{0}", path);
                    }
                }
                if (!string.IsNullOrEmpty(_error)&&_mainForm!=null)
                {
                    _mainForm.ShowMessage(_error);
                }
            }
           
        }
    }
}

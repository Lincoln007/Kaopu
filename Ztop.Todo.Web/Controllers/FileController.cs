using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Common;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Controllers
{
    public class FileController : ControllerBase
    {
        [HttpPost]
        public ActionResult Upload(TodoFile todofile,string addfolder)
        {
            if (Request.Files.Count == 0)
            {
                throw new ArgumentException("请选择上传文件");
            }
            var file = HttpContext.Request.Files[0];
            var fileName = file.FileName;
            var ext = System.IO.Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("请选择上传文件");
            }
            string saveFilePath = string.Empty;
            switch (todofile)
            {
                case TodoFile.Contract:
                    if (ext != ".pdf")
                    {
                        throw new ArgumentException("请上传pdf文件");
                    }
                    saveFilePath = FileManager.UploadContract(file,addfolder);
                    break;
                case TodoFile.iPad_Contract:
                    //saveFilePath = FileManager.Upload2(file);
                    break;
            }

            return SuccessJsonResult(new { saveFilePath, fileName });
        }
    }
}
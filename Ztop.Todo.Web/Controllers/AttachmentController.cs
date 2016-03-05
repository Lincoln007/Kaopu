using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ztop.Todo.Web.Controllers
{
    public class AttachmentController : ControllerBase
    {
        // GET: Attachment
        public ActionResult Download(int id)
        {
            var model = Core.AttachmentManager.GetModel(id);
            if (model == null)
            {
                throw new ArgumentException("参数错误");
            }

            var fileData = Core.AttachmentManager.GetFileData(model);
            var contentType = WebUtility.GetContentType(model.FileName);

            return File(fileData, contentType, model.FileName);
        }
    }
}
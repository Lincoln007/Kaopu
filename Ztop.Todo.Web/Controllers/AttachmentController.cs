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

        public ActionResult Delete(int id)
        {
            var file = Core.AttachmentManager.GetModel(id);
            if (file != null)
            {
                if (Core.TaskManager.HasRight(file.TaskID, Identity.UserID))
                {
                    Core.AttachmentManager.Delete(file.ID);
                }
                else
                {
                    throw new HttpException(401, "没有权限删除该附件");
                }
            }
            throw new ArgumentException("参数错误");
        }
    }
}
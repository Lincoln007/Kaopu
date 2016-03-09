using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;
using Ztop.Todo.Common;

namespace Ztop.Todo.Web.Controllers
{
    public class QueryController : ControllerBase
    {
        public ActionResult Save(string name, string keyword, bool isCreator = true, int days = 0, UserTaskOrder order = UserTaskOrder.CreateTime, bool? isCompleted = null, int queryId = 0)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("请填写任务名称");
            }

            TaskQueryParameter parameter = null;
            TaskQuery model = null;

            if (queryId > 0)
            {
                model = Core.QueryManager.GetModel(queryId);
                if (model == null)
                {
                    throw new ArgumentException("参数错误");
                }

                if (model.UserID != Identity.UserID)
                {
                    throw new HttpException(401, "没有权限");
                }
                parameter = model.ConvertToTaskQueryParameter();
            }
            else
            {
                parameter = new TaskQueryParameter
                {
                    SearchKey = keyword,
                    IsCreator = isCreator,
                    ReceiverID = isCreator ? 0 : Identity.UserID,
                    IsCompleted = isCompleted,
                    Order = order,
                    GetReceiver = true,
                    GetCreator = true,
                };
                model = new TaskQuery
                {
                    UserID = Identity.UserID,
                };
            }

            model.Content = parameter.ToJson();
            model.Name = name;

            Core.QueryManager.Save(model);
            return SuccessJsonResult(new { id = model.ID });
        }

        public ActionResult Delete(int queryId)
        {
            Core.QueryManager.Delete(queryId, Identity.UserID);
            return SuccessJsonResult();
        }
    }
}
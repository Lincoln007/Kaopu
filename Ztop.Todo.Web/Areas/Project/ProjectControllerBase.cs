using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ztop.Todo.Model;

namespace Ztop.Todo.Web.Areas.Project
{
    [UserAuthorize]
    public class ProjectControllerBase : Ztop.Todo.Web.Controllers.ControllerBase
    {
        private Floww _floww { get; set; }
        public Floww Floww { get { return _floww == null ? _floww = Core.FlowwManager.Get(1) : _floww; } }
    }
}
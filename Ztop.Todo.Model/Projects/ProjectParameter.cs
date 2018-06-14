using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Model
{
    public class ProjectParameter:ParameterBase
    {
        /// <summary>
        /// 项目名称关键字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 乡镇主体
        /// </summary>
        public string Town { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int? Year { get; set; }
        /// <summary>
        /// 城市关键字
        /// </summary>
        public string CityName { get; set; }
        public int? CityId { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string GroupName { get; set; }

        public int? GroupId { get; set; }
        public int? FID { get; set; }
        public int? SEID { get; set; }
        public ProjectOrder Order { get; set; }
        public int ? ChargeID { get; set; }
        public bool? IsRecord { get; set; }
        public int? PartId { get; set; }
        public string Participation { get; set; }

    }
}

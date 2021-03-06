﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("errands")]
    public class Errand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 起始日期
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Users { get; set; }
        public int Peoples { get; set; }
        public int Days { get; set; }
        public int EID { get; set; }

        [NotMapped]
        public string Time
        {
            get
            {
                return string.Format("{0}至{1}", StartTime.ToString("yyyy-MM-dd"), EndTime.ToString("yyyy-MM-dd"));
            }
        }
    }
}

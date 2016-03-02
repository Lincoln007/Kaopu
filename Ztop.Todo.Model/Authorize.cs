﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Model
{
    [Table("authorizes")]
    public class Authorize
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string GroupName { get; set; }
        public string Manager { get; set; }
    }

    public enum AuthFilter
    {
        All,
        Wait,
        Agree,
        Disagree
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class SheetManager:ManagerBase
    {

        public Sheet GetModel(int id)
        {
            if (id == 0) return null;
            using (var db = GetDbContext())
            {
                return db.Sheets.FirstOrDefault(e => e.ID == id);
            }
        }
        public Sheet GetSerialNumberModel(int id)
        {
            if (id == 0)
            {
                return new Sheet
                {
                    SerialNumber = Core.SerialNumberManager.GetNewModel()
                };
            }
            var sheet = GetAllModel(id);
            if (sheet == null)
            {
                throw new ArgumentException("参数错误，未找到相关报销单信息");
            }
            return sheet;

        }
        public Sheet GetAllModel(int id)
        {
            if (id == 0) return null;
            using (var db = GetDbContext())
            {
                var model = db.Sheets.FirstOrDefault(e => e.ID == id);
                if (model != null)
                {
                    model.SerialNumber = db.SerialNumbers.FirstOrDefault(e => e.SID == model.ID);
                    model.Substances = db.Substances.Where(e => e.SID == id).OrderBy(e=>e.ID).ToList();
                    model.Verifys = db.Verifys.Where(e => e.SID == id).OrderBy(e => e.ID).ToList();
                }
                return model;
            }
        }
        /// <summary>
        /// 用于保存报销单
        /// </summary>
        /// <param name="sheet"></param>
        public void Save(Sheet sheet)
        {
            if (sheet == null) return;
            using (var db = GetDbContext())
            {
                if (sheet.ID == 0)
                {
                    db.Sheets.Add(sheet);
                }
                else
                {
                    var entry = db.Sheets.FirstOrDefault(e => e.ID == sheet.ID);
                    if (entry != null)
                    {
                        db.Entry(entry).CurrentValues.SetValues(sheet);
                    }
                }
                db.SaveChanges();
                if (sheet.Substances != null)
                {
                    var older = db.Substances.Where(e => e.SID == sheet.ID).ToList();//如果重新编辑了报销单，则删除之前所有的子清单
                    if (older != null)
                    {
                        db.Substances.RemoveRange(older);
                        db.SaveChanges();
                    }
                    
                    db.Substances.AddRange(sheet.Substances.OrderBy(e => e.ID).Select(e => new Substancs
                    {
                        Category = e.Category,
                        Details = e.Details,
                        Price = e.Price,
                        SID = sheet.ID
                    }));
                }
                
                db.SaveChanges();

            }
        }
        private SerialNumber GetSerialNumber(int sid)
        {
            if (sid == 0) return null;
            using (var db = GetDbContext())
            {
                return db.SerialNumbers.FirstOrDefault(e => e.SID == sid);
            }
        }
        private List<Sheet> GetSheets()
        {
            using (var db = GetDbContext())
            {
                return db.Sheets.ToList();
            }
        }
        private List<Sheet> GetSerialNumberSheets()
        {
            var list = GetSheets();
            foreach(var sheet in list)
            {
                sheet.SerialNumber = GetSerialNumber(sheet.ID);
            }
            return list;
        }
        public List<Sheet> GetSheets(SheetQueryParameter parameter)
        {
            var list = GetSerialNumberSheets();
            var query = list.AsQueryable();
            if (!string.IsNullOrEmpty(parameter.Name))
            {
                query = query.Where(e => e.Name == parameter.Name);
            }
            if (!string.IsNullOrEmpty(parameter.Controler))
            {
                query = query.Where(e => e.Controler == parameter.Controler);
            }
            if (parameter.Deleted.HasValue)
            {
                query = query.Where(e => e.Deleted == parameter.Deleted.Value);
            }
            if (parameter.Status.HasValue)
            {
                query = query.Where(e => e.Status == parameter.Status.Value);
            }
            return query.ToList();
        }

        public List<Sheet> GetSheets(QueryParameter parameter,string name)
        {
            var list = GetSerialNumberSheets();
            var query = list.AsQueryable();
            if (parameter.Creater == Operator.我)
            {
                query = query.Where(e => e.Name == name);
            }else if (parameter.Creater == Operator.自定义)
            {
                query = query.Where(e => e.Name == parameter.Custom.Trim().ToUpper());
            }

            switch (parameter.Status)
            {
                case StatusPosition.草稿:
                    query = query.Where(e => e.Status == Status.OutLine);
                    break;
                case StatusPosition.未审核:
                    query = query.Where(e => e.Status == Status.ExaminingDirector || e.Status == Status.ExaminingManager || e.Status == Status.ExaminingFinance);
                    break;
                case StatusPosition.已审核:
                    query = query.Where(e => e.Status == Status.Examined);
                    break;
            }
            var days = 0;
            switch (parameter.Time)
            {
                case Time.一周内:
                    days = 7;
                    break;
                case Time.一年内:
                    days = 365;
                    break;
                case Time.一月内:
                    days = 31;
                    break;
                case Time.半年内:
                    days = 183;
                    break;
            }
            if (days != 0)
            {
                var time = DateTime.Now.AddDays(days);
                query = query.Where(e => e.Time <= time);
            }
            if (parameter.Order == Order.Time)
            {
                query = query.OrderBy(e => e.Time);
            }
            else
            {
                query = query.OrderBy(e => e.Money);
            }
            return query.ToList();
        }
        public void Delete(int id)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Sheets.FirstOrDefault(e => e.ID == id);
                if (entry != null)
                {
                    entry.Deleted = true;
                    db.SaveChanges();
                }
            }
        }
    }
}

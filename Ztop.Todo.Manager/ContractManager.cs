using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ztop.Todo.Model;

namespace Ztop.Todo.Manager
{
    public class ContractManager:ManagerBase
    {
        private static string _directory = "ContractFiles";
        private bool Exist(Contract contract)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Contracts.FirstOrDefault(e => e.Coding == contract.Coding && e.Name == contract.Name);
                return entry != null;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public int Save(Contract contract)
        {
            if (Exist(contract))
            {
                throw new Exception(string.Format("当前系统存在合同编号：{0} 合同名称：{1}", contract.Coding, contract.Name));
            }

            using (var db = GetDbContext())
            {
                db.Contracts.Add(contract);
                db.SaveChanges();
                return contract.ID;
            }

        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public int Edit(Contract contract)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Contracts.Find(contract.ID);
                if (entry != null)
                {
                    db.Entry(entry).CurrentValues.SetValues(contract);
                    db.SaveChanges();
                }
                return contract.ID;
            }
        }

        public bool Delete(int id)
        {
            using (var db = GetDbContext())
            {
                var entry = db.Contracts.Find(id);
                if (entry == null)
                {
                    return false;
                }
                entry.Deleted = true;
                db.SaveChanges();
                return true;
            }
        }

        public bool Archive(int id)
        {
            if (id == 0)
            {
                return false;
            }
            using (var db = GetDbContext())
            {
                var entry = db.Contracts.Find(id);
                if (entry == null)
                {
                    return false;
                }
                entry.Archived = true;
                db.SaveChanges();
                return true;
            }
        }


        public Contract Get(int id)
        {
            using (var db = GetDbContext())
            {
                return db.Contracts.Find(id);
            }
        }

        public List<Contract> Get()
        {
            using (var db = GetDbContext())
            {
                return db.Contracts.Where(e=>e.Deleted==false).ToList();
            }
        }

        public List<Contract> Search(ContractParameter parameter)
        {
            using (var db = GetDbContext())
            {
                var query = db.Contracts.AsQueryable();
                if (parameter.Archived.HasValue)
                {
                    query = query.Where(e => e.Archived == parameter.Archived.Value);
                }
                if (!string.IsNullOrEmpty(parameter.Name))
                {
                    query = query.Where(e => e.Name.Contains(parameter.Name));
                }
                if (!string.IsNullOrEmpty(parameter.OtherSide))
                {
                    query = query.Where(e => e.Company.Contains(parameter.OtherSide));
                }
                if (parameter.ZtopCompany.HasValue)
                {
                    query = query.Where(e => e.ZtopCompany == parameter.ZtopCompany.Value);
                }
                query = query.OrderByDescending(e=>e.Coding).SetPage(parameter.Page);
                return query.ToList();
            }
        }

        public void SaveContractFile(HttpContextBase context,int cid)
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
           
            if (context.Request.Files.Count > 0)
            {
                var list = new List<ContractFile>();
                for(var i = 0; i < context.Request.Files.Count; i++)
                {
                    var file = context.Request.Files[i];
                    if (file.ContentLength > 0)
                    {
                        var ext = System.IO.Path.GetExtension(file.FileName);
                        var fileName = file.FileName.Replace(ext, "") + DateTime.Now.Ticks.ToString() + ext;
                        if (fileName.Length > 100)
                        {
                            fileName = fileName.Substring(fileName.Length - 100);
                        }
                        var savePath = System.IO.Path.Combine(_directory, fileName);
                        file.SaveAs(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,savePath));
                        list.Add(new ContractFile()
                        {
                            ContractID = cid,
                            SavePath = savePath,
                            FileName = file.FileName,
                            FileSize = file.ContentLength
                        });
                    }
                }
                if (list.Count > 0)
                {
                    using (var db = GetDbContext())
                    {
                        db.ContractFiles.AddRange(list);
                        db.SaveChanges();
                    }
                }
            }
        }
        public List<ContractFile> GetContractFiles(int cid)
        {
            using (var db = GetDbContext())
            {
                return db.ContractFiles.Where(e => e.ContractID == cid).ToList();
            }
        }

    }
}

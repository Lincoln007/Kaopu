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
    public class ContractFileManager:ManagerBase
    {
        private static string _directory = "ContractFiles";
        public void SaveContractFile(HttpContextBase context, int cid)
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }

            if (context.Request.Files.Count > 0)
            {
                var list = new List<ContractFile>();
                for (var i = 0; i < context.Request.Files.Count; i++)
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
                        file.SaveAs(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, savePath));
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
                return db.ContractFiles.Where(e => e.ContractID == cid&&e.Remove==false).ToList();
            }
        }

        public void Update(int[] hasid,int contractid)
        {
            using (var db = GetDbContext())
            {
                var list = db.ContractFiles.Where(e => e.ContractID == contractid).ToList();
                foreach(var contractfile in list)
                {
                    if (!hasid.Contains(contractfile.ID))
                    {
                        contractfile.Remove = true;
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}

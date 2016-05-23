using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Ztop.Todo.Update
{
    public class ZipClass
    {
        public delegate void SetProgressDelegate(int maximum, string msg);

        /// <summary>
        /// 解压zip格式的文件
        /// </summary>
        /// <param name="zipfilePath">压缩文件路径，全路径格式</param>
        /// <param name="unZipDir">解压文件存放路径，全路径格斯，为空时默认与压缩文件同一级目录下，跟解压文件同名的文件夹</param>
        /// <param name="maximum"></param>
        /// <param name="setProgressDelegate"></param>
        /// <returns></returns>
        public static bool UnZip(string zipFilePath,string unZipDir,int maximum,SetProgressDelegate setProgressDelegate)
        {
            if (string.IsNullOrEmpty(zipFilePath))
            {
                throw new System.IO.FileNotFoundException("压缩文件不能为空！");
            }
            if (!File.Exists(zipFilePath))
            {
                throw new System.IO.FileNotFoundException(string.Format("压缩文件：{0} 不存在", zipFilePath));
            }
            if (string.IsNullOrEmpty(unZipDir))
            {
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), "");
            }
            if (!unZipDir.EndsWith("//"))
            {
                unZipDir += "//";
            }

            if (!Directory.Exists(unZipDir))
            {
                Directory.CreateDirectory(unZipDir);
            }
            try
            {
                using (ZipInputStream s=new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(unZipDir + directoryName);
                        }
                        if (!directoryName.EndsWith("//"))
                        {
                            directoryName += "//";
                        }
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            using (FileStream fs = File.Create(unZipDir + theEntry.Name))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        fs.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        setProgressDelegate(maximum, theEntry.Name);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
                
            }
            return true;
        }

        public static int GetMaximum(string[] zips)
        {
            int maximum = 0;
            ZipInputStream s = null;
            ZipEntry theEntry = null;
            foreach(var zip in zips)
            {
                s = new ZipInputStream(File.OpenRead(System.IO.Path.Combine(Application.StartupPath, zip)));
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (!string.IsNullOrEmpty(Path.GetFileName(theEntry.Name)))
                    {
                        maximum++;
                    }
                }
            }
            if (s != null)
            {
                s.Close();
            }
            return maximum;
        }
    }
}

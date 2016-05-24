using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Ztop.Todo.Update.UPDATE;

namespace Ztop.Todo.Update
{
    public partial class UpdateForm : Form
    {
        public delegate void CloseProgressDelegate();
        public event CloseProgressDelegate CloseProgress;

        public Service Service { get; set; }
        public WebClient WC { get; set; }
        /// <summary>
        /// 获取下载地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 获取升级压缩包
        /// </summary>
        public string[] Zips { get; set; }
        /// <summary>
        /// 当前正在下载的压缩包
        /// </summary>
        public string Zip { get; set; }
        /// <summary>
        /// 当前正在下载的zips下标
        /// </summary>
        public int ZipsIndex { get; set; }
        /// <summary>
        /// 上一次下载流量
        /// </summary>
        public long PreBytes { get; set; }
        /// <summary>
        /// 当前下载流量
        /// </summary>
        public long CurrentBytes { get; set; }
        public UpdateForm()
        {
            InitializeComponent();
            Service = new Service();
            Url = Service.GetUrl();
            Zips = Service.GetZips();
        }
        

        /// <summary>
        /// 实现关闭进度条事件
        /// </summary>
        private void FrmUpdate_closeProgress()
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new CloseProgressDelegate(FrmUpdate_closeProgress));
            }
            else
            {
                if (WC != null)
                {
                    WC.Dispose();
                }
                if (Zips.Length > 0)
                {
                    MessageBox.Show("升级成功!");
                }
                else
                {
                    MessageBox.Show("未找到升级包！");
                }

                IniClass ini = new IniClass(System.IO.Path.Combine(Application.StartupPath, System.Configuration.ConfigurationManager.ConnectionStrings["UPDATE"].ConnectionString));
                string serviceVersion = Service.GetVersion();//服务端版本
                ini.IniWriteValue("update", "version", serviceVersion);//更新成功后将版本写入配置文件
                Application.Exit();
                Process.Start(System.Configuration.ConfigurationManager.ConnectionStrings["MAIN"].ConnectionString);
            }
        }

        /// <summary>
        /// 下载时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wc_downloadProgressChanged(object sender,DownloadProgressChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DownloadProgressChangedEventHandler(wc_downloadProgressChanged), new object[] { sender, e });
            }
            else
            {
                if (ZipsIndex < Zip.Length)
                {
                    label1.Text = string.Format("正在下载自解压缩包{0}（{1}/{2})", Zips[ZipsIndex], (ZipsIndex + 1).ToString(), Zips.Length);
                    progressBar1.Maximum = 100;
                    progressBar1.Value = e.ProgressPercentage;
                    CurrentBytes = e.BytesReceived;//当前下载流量
                }
            }
        }

        /// <summary>
        /// 文件下载完成（其中一个文件下载成功）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wc_downloadFileCompleted(object sender,AsyncCompletedEventArgs e)
        {
            ZipsIndex++;
            if (ZipsIndex < Zips.Length)
            {
                WC = new WebClient();
                WC.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_downloadProgressChanged);
                WC.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_downloadFileCompleted);
                WC.DownloadFileAsync(new Uri(Url + Zips[ZipsIndex]), Zips[ZipsIndex]);
            }
            else
            {
                int maximum = ZipClass.GetMaximum(Zips);
                foreach(var zip in Zips)
                {
                    var zipPath = System.IO.Path.Combine(Application.StartupPath, zip);
                    ZipClass.UnZip(zipPath, "", maximum , FrmUpdate_SetProgress);
                    System.IO.File.Delete(zipPath);
                }
                FrmUpdate_closeProgress();//调用关闭进度条事件
            }
        }

        /// <summary>
        /// 解压时进度条事件
        /// </summary>
        /// <param name="maximum">进度条最大值</param>
        /// <param name="msg"></param>
        private void FrmUpdate_SetProgress(int maximum,string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ZipClass.SetProgressDelegate(FrmUpdate_SetProgress), new object[] { maximum, msg });
            }
            else
            {
                timer1.Enabled = false;
                this.Text = "升级";
                if (ZipsIndex == Zips.Length)
                {
                    progressBar1.Value = 0;
                    ZipsIndex++;
                }
                label1.Text = string.Format("正在解压{0}（{1}/{2}）", msg, (progressBar1.Value + 1).ToString(), maximum);
                progressBar1.Maximum = maximum;
                progressBar1.Value++;
            }
        }


        private void Download()
        {
            try
            {
                CloseProgress += new CloseProgressDelegate(FrmUpdate_closeProgress);
                if (Zips.Length > 0)
                {
                    WC = new WebClient();
                    WC.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_downloadProgressChanged);
                    WC.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_downloadFileCompleted);
                    WC.DownloadFileAsync(new Uri(Url + Zips[ZipsIndex]), Zips[ZipsIndex]);
                }
                else
                {
                    FrmUpdate_closeProgress();//调用关闭进度条事件
                }

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = ((CurrentBytes - PreBytes) / 1024).ToString() + "kb/s";
            PreBytes = CurrentBytes;
        }
    }
}

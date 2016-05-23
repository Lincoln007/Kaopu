using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        

        private void FrmUpdate_closeProgress()
        {

            if (this.InvokeRequired)
            {

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

            }
        }


        private void Download()
        {
            try
            {
                //CloseProgress+=new CloseProgressDelegate

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

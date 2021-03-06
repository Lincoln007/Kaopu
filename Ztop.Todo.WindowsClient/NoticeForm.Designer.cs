﻿namespace Ztop.Todo.WindowsClient
{
    partial class NoticeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtContent = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.ForeColor = System.Drawing.Color.Crimson;
            this.txtContent.Location = new System.Drawing.Point(0, 0);
            this.txtContent.Name = "txtContent";
            this.txtContent.Padding = new System.Windows.Forms.Padding(10);
            this.txtContent.Size = new System.Drawing.Size(206, 117);
            this.txtContent.TabIndex = 9;
            this.txtContent.Text = "郑良军  下达了  测试新的通知任务";
            this.txtContent.Click += new System.EventHandler(this.labContent_Click);
            // 
            // txtTime
            // 
            this.txtTime.AutoSize = true;
            this.txtTime.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtTime.Location = new System.Drawing.Point(0, 87);
            this.txtTime.Name = "txtTime";
            this.txtTime.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.txtTime.Size = new System.Drawing.Size(57, 30);
            this.txtTime.TabIndex = 10;
            this.txtTime.Text = "时间";
            // 
            // NoticeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(206, 117);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.txtContent);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NoticeForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "任务提醒";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NoticeForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label txtContent;
        private System.Windows.Forms.Label txtTime;
    }
}
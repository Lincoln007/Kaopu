namespace Ztop.Todo.WindowsClient
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
            this.labOwner = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnTitle = new System.Windows.Forms.LinkLabel();
            this.labContent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labOwner
            // 
            this.labOwner.AutoSize = true;
            this.labOwner.Location = new System.Drawing.Point(13, 135);
            this.labOwner.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labOwner.Name = "labOwner";
            this.labOwner.Size = new System.Drawing.Size(107, 20);
            this.labOwner.TabIndex = 2;
            this.labOwner.Text = "发布者：郑良军";
            // 
            // btnTitle
            // 
            this.btnTitle.ActiveLinkColor = System.Drawing.Color.Green;
            this.btnTitle.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnTitle.Location = new System.Drawing.Point(15, 9);
            this.btnTitle.Name = "btnTitle";
            this.btnTitle.Size = new System.Drawing.Size(187, 21);
            this.btnTitle.TabIndex = 8;
            this.btnTitle.TabStop = true;
            this.btnTitle.Text = "您有新的任务 1 条";
            this.btnTitle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnTitle_LinkClicked);
            // 
            // labContent
            // 
            this.labContent.Location = new System.Drawing.Point(15, 30);
            this.labContent.Name = "labContent";
            this.labContent.Size = new System.Drawing.Size(186, 100);
            this.labContent.TabIndex = 9;
            this.labContent.Click += new System.EventHandler(this.labContent_Click);
            // 
            // NoticeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(220, 170);
            this.Controls.Add(this.btnTitle);
            this.Controls.Add(this.labContent);
            this.Controls.Add(this.labOwner);
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
        private System.Windows.Forms.Label labOwner;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.LinkLabel btnTitle;
        private System.Windows.Forms.Label labContent;
    }
}
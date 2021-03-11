
namespace McInstaller
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.L_SysInfo = new System.Windows.Forms.Label();
            this.CB_JreInstalled = new System.Windows.Forms.CheckBox();
            this.CB_LauncherDownloaded = new System.Windows.Forms.CheckBox();
            this.CB_JreDownloaded = new System.Windows.Forms.CheckBox();
            this.CB_LauncherConfig = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(169, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "系统位数:";
            // 
            // L_SysInfo
            // 
            this.L_SysInfo.AutoSize = true;
            this.L_SysInfo.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.L_SysInfo.Location = new System.Drawing.Point(260, 33);
            this.L_SysInfo.Name = "L_SysInfo";
            this.L_SysInfo.Size = new System.Drawing.Size(48, 19);
            this.L_SysInfo.TabIndex = 0;
            this.L_SysInfo.Text = "64位";
            // 
            // CB_JreInstalled
            // 
            this.CB_JreInstalled.AutoSize = true;
            this.CB_JreInstalled.Enabled = false;
            this.CB_JreInstalled.Location = new System.Drawing.Point(188, 114);
            this.CB_JreInstalled.Name = "CB_JreInstalled";
            this.CB_JreInstalled.Size = new System.Drawing.Size(90, 16);
            this.CB_JreInstalled.TabIndex = 1;
            this.CB_JreInstalled.Text = "JRE安装完成";
            this.CB_JreInstalled.UseVisualStyleBackColor = true;
            // 
            // CB_LauncherDownloaded
            // 
            this.CB_LauncherDownloaded.AutoSize = true;
            this.CB_LauncherDownloaded.Enabled = false;
            this.CB_LauncherDownloaded.Location = new System.Drawing.Point(188, 149);
            this.CB_LauncherDownloaded.Name = "CB_LauncherDownloaded";
            this.CB_LauncherDownloaded.Size = new System.Drawing.Size(108, 16);
            this.CB_LauncherDownloaded.TabIndex = 2;
            this.CB_LauncherDownloaded.Text = "启动器下载完成";
            this.CB_LauncherDownloaded.UseVisualStyleBackColor = true;
            // 
            // CB_JreDownloaded
            // 
            this.CB_JreDownloaded.AutoSize = true;
            this.CB_JreDownloaded.Enabled = false;
            this.CB_JreDownloaded.Location = new System.Drawing.Point(188, 82);
            this.CB_JreDownloaded.Name = "CB_JreDownloaded";
            this.CB_JreDownloaded.Size = new System.Drawing.Size(90, 16);
            this.CB_JreDownloaded.TabIndex = 1;
            this.CB_JreDownloaded.Text = "JRE下载完成";
            this.CB_JreDownloaded.UseVisualStyleBackColor = true;
            // 
            // CB_LauncherConfig
            // 
            this.CB_LauncherConfig.AutoSize = true;
            this.CB_LauncherConfig.Enabled = false;
            this.CB_LauncherConfig.Location = new System.Drawing.Point(188, 185);
            this.CB_LauncherConfig.Name = "CB_LauncherConfig";
            this.CB_LauncherConfig.Size = new System.Drawing.Size(108, 16);
            this.CB_LauncherConfig.TabIndex = 2;
            this.CB_LauncherConfig.Text = "启动器安装完成";
            this.CB_LauncherConfig.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 310);
            this.Controls.Add(this.CB_LauncherConfig);
            this.Controls.Add(this.CB_LauncherDownloaded);
            this.Controls.Add(this.CB_JreDownloaded);
            this.Controls.Add(this.CB_JreInstalled);
            this.Controls.Add(this.L_SysInfo);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "MineCraft Java版安装器";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label L_SysInfo;
        private System.Windows.Forms.CheckBox CB_JreInstalled;
        private System.Windows.Forms.CheckBox CB_LauncherDownloaded;
        private System.Windows.Forms.CheckBox CB_JreDownloaded;
        private System.Windows.Forms.CheckBox CB_LauncherConfig;
    }
}


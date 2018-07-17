namespace Maple.NetWorkTools
{
    partial class FrmMain
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pgbPortScanerProgress = new System.Windows.Forms.ProgressBar();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPortScanerTimeOut = new System.Windows.Forms.TextBox();
            this.tbPortScanerTimeOut = new System.Windows.Forms.TrackBar();
            this.btnPortScanerStop = new System.Windows.Forms.Button();
            this.btnPortScanerStart = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rtxtPortScanerStatus = new System.Windows.Forms.RichTextBox();
            this.lstbPortScanerStatus = new System.Windows.Forms.ListBox();
            this.cbShowEnableIp = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtPortScanerTaskNum = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbPortScanerSinglePort = new System.Windows.Forms.CheckBox();
            this.lbPortScanerPort = new System.Windows.Forms.Label();
            this.txtPortScanerPortEnd = new System.Windows.Forms.TextBox();
            this.txtPortScanerPortStart = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbPortScanerSingleIp = new System.Windows.Forms.CheckBox();
            this.txtPortScanerIpEnd = new System.Windows.Forms.TextBox();
            this.lbPortScanerIp = new System.Windows.Forms.Label();
            this.txtPortScanerIpStart = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPortScanerTimeOut)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pgbPortScanerProgress);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.btnPortScanerStop);
            this.tabPage1.Controls.Add(this.btnPortScanerStart);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pgbPortScanerProgress
            // 
            resources.ApplyResources(this.pgbPortScanerProgress, "pgbPortScanerProgress");
            this.pgbPortScanerProgress.Name = "pgbPortScanerProgress";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.txtPortScanerTimeOut);
            this.groupBox5.Controls.Add(this.tbPortScanerTimeOut);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtPortScanerTimeOut
            // 
            resources.ApplyResources(this.txtPortScanerTimeOut, "txtPortScanerTimeOut");
            this.txtPortScanerTimeOut.Name = "txtPortScanerTimeOut";
            this.txtPortScanerTimeOut.ReadOnly = true;
            // 
            // tbPortScanerTimeOut
            // 
            resources.ApplyResources(this.tbPortScanerTimeOut, "tbPortScanerTimeOut");
            this.tbPortScanerTimeOut.Name = "tbPortScanerTimeOut";
            this.tbPortScanerTimeOut.Scroll += new System.EventHandler(this.tbPortScanerTimeOut_Scroll);
            // 
            // btnPortScanerStop
            // 
            resources.ApplyResources(this.btnPortScanerStop, "btnPortScanerStop");
            this.btnPortScanerStop.Name = "btnPortScanerStop";
            this.btnPortScanerStop.UseVisualStyleBackColor = true;
            this.btnPortScanerStop.Click += new System.EventHandler(this.btnPortScanerStop_Click);
            // 
            // btnPortScanerStart
            // 
            resources.ApplyResources(this.btnPortScanerStart, "btnPortScanerStart");
            this.btnPortScanerStart.Name = "btnPortScanerStart";
            this.btnPortScanerStart.UseVisualStyleBackColor = true;
            this.btnPortScanerStart.Click += new System.EventHandler(this.btnPortScanerStart_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rtxtPortScanerStatus);
            this.groupBox4.Controls.Add(this.lstbPortScanerStatus);
            this.groupBox4.Controls.Add(this.cbShowEnableIp);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // rtxtPortScanerStatus
            // 
            resources.ApplyResources(this.rtxtPortScanerStatus, "rtxtPortScanerStatus");
            this.rtxtPortScanerStatus.Name = "rtxtPortScanerStatus";
            // 
            // lstbPortScanerStatus
            // 
            this.lstbPortScanerStatus.FormattingEnabled = true;
            resources.ApplyResources(this.lstbPortScanerStatus, "lstbPortScanerStatus");
            this.lstbPortScanerStatus.Name = "lstbPortScanerStatus";
            // 
            // cbShowEnableIp
            // 
            resources.ApplyResources(this.cbShowEnableIp, "cbShowEnableIp");
            this.cbShowEnableIp.Name = "cbShowEnableIp";
            this.cbShowEnableIp.UseVisualStyleBackColor = true;
            this.cbShowEnableIp.CheckedChanged += new System.EventHandler(this.cbShowEnableIp_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtPortScanerTaskNum);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // txtPortScanerTaskNum
            // 
            resources.ApplyResources(this.txtPortScanerTaskNum, "txtPortScanerTaskNum");
            this.txtPortScanerTaskNum.Name = "txtPortScanerTaskNum";
            this.txtPortScanerTaskNum.TextChanged += new System.EventHandler(this.txtPortScanerTaskNum_TextChanged);
            this.txtPortScanerTaskNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPortScanerTaskNum_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbPortScanerSinglePort);
            this.groupBox2.Controls.Add(this.lbPortScanerPort);
            this.groupBox2.Controls.Add(this.txtPortScanerPortEnd);
            this.groupBox2.Controls.Add(this.txtPortScanerPortStart);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cbPortScanerSinglePort
            // 
            resources.ApplyResources(this.cbPortScanerSinglePort, "cbPortScanerSinglePort");
            this.cbPortScanerSinglePort.Name = "cbPortScanerSinglePort";
            this.cbPortScanerSinglePort.UseVisualStyleBackColor = true;
            this.cbPortScanerSinglePort.CheckedChanged += new System.EventHandler(this.cbPortScanerSinglePort_CheckedChanged);
            // 
            // lbPortScanerPort
            // 
            resources.ApplyResources(this.lbPortScanerPort, "lbPortScanerPort");
            this.lbPortScanerPort.Name = "lbPortScanerPort";
            // 
            // txtPortScanerPortEnd
            // 
            resources.ApplyResources(this.txtPortScanerPortEnd, "txtPortScanerPortEnd");
            this.txtPortScanerPortEnd.Name = "txtPortScanerPortEnd";
            this.txtPortScanerPortEnd.TextChanged += new System.EventHandler(this.txtPortScanerPortEnd_TextChanged);
            this.txtPortScanerPortEnd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPortScanerPortEnd_KeyPress);
            // 
            // txtPortScanerPortStart
            // 
            resources.ApplyResources(this.txtPortScanerPortStart, "txtPortScanerPortStart");
            this.txtPortScanerPortStart.Name = "txtPortScanerPortStart";
            this.txtPortScanerPortStart.TextChanged += new System.EventHandler(this.txtPortScanerPortStart_TextChanged);
            this.txtPortScanerPortStart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPortScanerPortStart_KeyPress);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbPortScanerSingleIp);
            this.groupBox1.Controls.Add(this.txtPortScanerIpEnd);
            this.groupBox1.Controls.Add(this.lbPortScanerIp);
            this.groupBox1.Controls.Add(this.txtPortScanerIpStart);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cbPortScanerSingleIp
            // 
            resources.ApplyResources(this.cbPortScanerSingleIp, "cbPortScanerSingleIp");
            this.cbPortScanerSingleIp.Name = "cbPortScanerSingleIp";
            this.cbPortScanerSingleIp.UseVisualStyleBackColor = true;
            this.cbPortScanerSingleIp.CheckedChanged += new System.EventHandler(this.cbPortScanerSingleIp_CheckedChanged);
            // 
            // txtPortScanerIpEnd
            // 
            resources.ApplyResources(this.txtPortScanerIpEnd, "txtPortScanerIpEnd");
            this.txtPortScanerIpEnd.Name = "txtPortScanerIpEnd";
            this.txtPortScanerIpEnd.TextChanged += new System.EventHandler(this.txtPortScanerIpEnd_TextChanged);
            this.txtPortScanerIpEnd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPortScanerIpEnd_KeyPress);
            // 
            // lbPortScanerIp
            // 
            resources.ApplyResources(this.lbPortScanerIp, "lbPortScanerIp");
            this.lbPortScanerIp.Name = "lbPortScanerIp";
            // 
            // txtPortScanerIpStart
            // 
            resources.ApplyResources(this.txtPortScanerIpStart, "txtPortScanerIpStart");
            this.txtPortScanerIpStart.Name = "txtPortScanerIpStart";
            this.txtPortScanerIpStart.TextChanged += new System.EventHandler(this.txtPortScanerIpStart_TextChanged);
            this.txtPortScanerIpStart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPortScanerIpStart_KeyPress);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // FrmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPortScanerTimeOut)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ProgressBar pgbPortScanerProgress;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPortScanerTimeOut;
        private System.Windows.Forms.TrackBar tbPortScanerTimeOut;
        private System.Windows.Forms.Button btnPortScanerStop;
        private System.Windows.Forms.Button btnPortScanerStart;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RichTextBox rtxtPortScanerStatus;
        private System.Windows.Forms.ListBox lstbPortScanerStatus;
        private System.Windows.Forms.CheckBox cbShowEnableIp;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtPortScanerTaskNum;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbPortScanerSinglePort;
        private System.Windows.Forms.Label lbPortScanerPort;
        private System.Windows.Forms.TextBox txtPortScanerPortEnd;
        private System.Windows.Forms.TextBox txtPortScanerPortStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbPortScanerSingleIp;
        private System.Windows.Forms.TextBox txtPortScanerIpEnd;
        private System.Windows.Forms.Label lbPortScanerIp;
        private System.Windows.Forms.TextBox txtPortScanerIpStart;
        private System.Windows.Forms.Label label1;
    }
}


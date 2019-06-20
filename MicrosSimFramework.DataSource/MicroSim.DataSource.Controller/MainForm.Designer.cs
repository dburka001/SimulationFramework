namespace MicroSim.DataSource.Controller
{
    partial class MainForm
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
            this.listMsfDataSource = new System.Windows.Forms.ListBox();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.btnGenerateAgain = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.btnExport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // listMsfDataSource
            // 
            this.listMsfDataSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listMsfDataSource.FormattingEnabled = true;
            this.listMsfDataSource.ItemHeight = 31;
            this.listMsfDataSource.Location = new System.Drawing.Point(0, 0);
            this.listMsfDataSource.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.listMsfDataSource.Name = "listMsfDataSource";
            this.listMsfDataSource.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listMsfDataSource.Size = new System.Drawing.Size(473, 1027);
            this.listMsfDataSource.TabIndex = 0;
            this.listMsfDataSource.SelectedIndexChanged += new System.EventHandler(this.listMsfDataSourceSelectedIndexChanged);
            // 
            // mainPanel
            // 
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1429, 1149);
            this.mainPanel.TabIndex = 1;
            // 
            // rtbLog
            // 
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Location = new System.Drawing.Point(0, 0);
            this.rtbLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(419, 1149);
            this.rtbLog.TabIndex = 2;
            this.rtbLog.Text = "";
            // 
            // btnGenerateAgain
            // 
            this.btnGenerateAgain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGenerateAgain.Location = new System.Drawing.Point(-7, 1030);
            this.btnGenerateAgain.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnGenerateAgain.Name = "btnGenerateAgain";
            this.btnGenerateAgain.Size = new System.Drawing.Size(240, 119);
            this.btnGenerateAgain.TabIndex = 0;
            this.btnGenerateAgain.Text = "Adatok újra generálása";
            this.btnGenerateAgain.UseVisualStyleBackColor = true;
            this.btnGenerateAgain.Click += new System.EventHandler(this.btnGenerateAgainClick);
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer.Location = new System.Drawing.Point(480, 0);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.mainPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.rtbLog);
            this.splitContainer.Size = new System.Drawing.Size(1875, 1149);
            this.splitContainer.SplitterDistance = 1429;
            this.splitContainer.SplitterWidth = 27;
            this.splitContainer.TabIndex = 3;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.Location = new System.Drawing.Point(240, 1030);
            this.btnExport.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(240, 119);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExportClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2357, 1149);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.btnGenerateAgain);
            this.Controls.Add(this.listMsfDataSource);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.MainFormShown);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listMsfDataSource;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Button btnGenerateAgain;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button btnExport;
    }
}


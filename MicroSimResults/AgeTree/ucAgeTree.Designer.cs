namespace MicroSimResults
{
    partial class ucAgeTree
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainPanel = new System.Windows.Forms.Panel();
            this.hScrollYear = new System.Windows.Forms.HScrollBar();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.BackColor = System.Drawing.SystemColors.Control;
            this.mainPanel.Location = new System.Drawing.Point(0, 3);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(591, 327);
            this.mainPanel.TabIndex = 0;
            // 
            // hScrollYear
            // 
            this.hScrollYear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollYear.LargeChange = 1;
            this.hScrollYear.Location = new System.Drawing.Point(0, 333);
            this.hScrollYear.Name = "hScrollYear";
            this.hScrollYear.Size = new System.Drawing.Size(591, 29);
            this.hScrollYear.TabIndex = 2;
            this.hScrollYear.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollYear_Scroll);
            // 
            // ucAgeTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hScrollYear);
            this.Controls.Add(this.mainPanel);
            this.Name = "ucAgeTree";
            this.Size = new System.Drawing.Size(594, 362);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.HScrollBar hScrollYear;
    }
}

namespace MicroSimFramework
{
    partial class ucMainSettings
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
            this.lblStartYear = new System.Windows.Forms.Label();
            this.txtStartYear = new System.Windows.Forms.TextBox();
            this.txtEndYear = new System.Windows.Forms.TextBox();
            this.lblEndYear = new System.Windows.Forms.Label();
            this.lblMultiplier = new System.Windows.Forms.Label();
            this.chkUseHouseholds = new System.Windows.Forms.CheckBox();
            this.cboxMultiplierField = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblStartYear
            // 
            this.lblStartYear.AutoSize = true;
            this.lblStartYear.Location = new System.Drawing.Point(14, 19);
            this.lblStartYear.Name = "lblStartYear";
            this.lblStartYear.Size = new System.Drawing.Size(54, 13);
            this.lblStartYear.TabIndex = 0;
            this.lblStartYear.Text = "Start Year";
            // 
            // txtStartYear
            // 
            this.txtStartYear.Location = new System.Drawing.Point(106, 16);
            this.txtStartYear.Name = "txtStartYear";
            this.txtStartYear.Size = new System.Drawing.Size(121, 20);
            this.txtStartYear.TabIndex = 1;
            // 
            // txtEndYear
            // 
            this.txtEndYear.Location = new System.Drawing.Point(106, 42);
            this.txtEndYear.Name = "txtEndYear";
            this.txtEndYear.Size = new System.Drawing.Size(121, 20);
            this.txtEndYear.TabIndex = 3;
            // 
            // lblEndYear
            // 
            this.lblEndYear.AutoSize = true;
            this.lblEndYear.Location = new System.Drawing.Point(14, 45);
            this.lblEndYear.Name = "lblEndYear";
            this.lblEndYear.Size = new System.Drawing.Size(51, 13);
            this.lblEndYear.TabIndex = 2;
            this.lblEndYear.Text = "End Year";
            // 
            // lblMultiplier
            // 
            this.lblMultiplier.AutoSize = true;
            this.lblMultiplier.Location = new System.Drawing.Point(14, 71);
            this.lblMultiplier.Name = "lblMultiplier";
            this.lblMultiplier.Size = new System.Drawing.Size(70, 13);
            this.lblMultiplier.TabIndex = 4;
            this.lblMultiplier.Text = "MultiplierField";
            // 
            // chkUseHouseholds
            // 
            this.chkUseHouseholds.AutoSize = true;
            this.chkUseHouseholds.Location = new System.Drawing.Point(106, 108);
            this.chkUseHouseholds.Name = "chkUseHouseholds";
            this.chkUseHouseholds.Size = new System.Drawing.Size(104, 17);
            this.chkUseHouseholds.TabIndex = 6;
            this.chkUseHouseholds.Text = "Use Houesholds";
            this.chkUseHouseholds.UseVisualStyleBackColor = true;
            // 
            // cboxMultiplierField
            // 
            this.cboxMultiplierField.FormattingEnabled = true;
            this.cboxMultiplierField.Location = new System.Drawing.Point(106, 71);
            this.cboxMultiplierField.Name = "cboxMultiplierField";
            this.cboxMultiplierField.Size = new System.Drawing.Size(121, 21);
            this.cboxMultiplierField.TabIndex = 7;
            // 
            // ucMainSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboxMultiplierField);
            this.Controls.Add(this.chkUseHouseholds);
            this.Controls.Add(this.lblMultiplier);
            this.Controls.Add(this.txtEndYear);
            this.Controls.Add(this.lblEndYear);
            this.Controls.Add(this.txtStartYear);
            this.Controls.Add(this.lblStartYear);
            this.Name = "ucMainSettings";
            this.Size = new System.Drawing.Size(255, 143);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStartYear;
        private System.Windows.Forms.TextBox txtStartYear;
        private System.Windows.Forms.TextBox txtEndYear;
        private System.Windows.Forms.Label lblEndYear;
        private System.Windows.Forms.Label lblMultiplier;
        private System.Windows.Forms.CheckBox chkUseHouseholds;
        private System.Windows.Forms.ComboBox cboxMultiplierField;
    }
}

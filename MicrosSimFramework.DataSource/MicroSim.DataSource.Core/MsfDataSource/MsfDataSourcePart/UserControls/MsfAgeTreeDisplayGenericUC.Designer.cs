namespace MicroSim.DataSource.Core
{
    partial class MsfAgeTreeDisplayGenericUC<Tenum, Tdata, TsubData>
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
            this.displayPanel = new System.Windows.Forms.Panel();
            this.cbxEducation = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // displayPanel
            // 
            this.displayPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.displayPanel.Location = new System.Drawing.Point(0, 100);
            this.displayPanel.Name = "displayPanel";
            this.displayPanel.Size = new System.Drawing.Size(1092, 802);
            this.displayPanel.TabIndex = 0;
            // 
            // cbxEducation
            // 
            this.cbxEducation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cbxEducation.FormattingEnabled = true;
            this.cbxEducation.Location = new System.Drawing.Point(23, 19);
            this.cbxEducation.Name = "cbxEducation";
            this.cbxEducation.Size = new System.Drawing.Size(984, 54);
            this.cbxEducation.TabIndex = 1;
            this.cbxEducation.SelectedIndexChanged += new System.EventHandler(this.cbxEducationSelectedIndexChanged);
            // 
            // MsfAgeTreeEduDisplayUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbxEducation);
            this.Controls.Add(this.displayPanel);
            this.Name = "MsfAgeTreeEduDisplayUC";
            this.Size = new System.Drawing.Size(1092, 902);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel displayPanel;
        private System.Windows.Forms.ComboBox cbxEducation;
    }
}

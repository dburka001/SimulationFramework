namespace MicroSimFramework
{
    partial class ucConstants
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkMultipleValue = new System.Windows.Forms.CheckBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listConstants = new System.Windows.Forms.ListBox();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.txtStep = new System.Windows.Forms.TextBox();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(198, 54);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 25;
            // 
            // chkMultipleValue
            // 
            this.chkMultipleValue.AutoSize = true;
            this.chkMultipleValue.Location = new System.Drawing.Point(198, 84);
            this.chkMultipleValue.Name = "chkMultipleValue";
            this.chkMultipleValue.Size = new System.Drawing.Size(138, 17);
            this.chkMultipleValue.TabIndex = 23;
            this.chkMultipleValue.Text = "Simulate multiple values";
            this.chkMultipleValue.UseVisualStyleBackColor = true;
            this.chkMultipleValue.CheckedChanged += new System.EventHandler(this.chkMultipleValue_CheckedChanged);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(240, 142);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 39);
            this.btnRemove.TabIndex = 22;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(159, 142);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 39);
            this.btnAdd.TabIndex = 21;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(156, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "label4";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(156, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "label2";
            // 
            // listConstants
            // 
            this.listConstants.FormattingEnabled = true;
            this.listConstants.Location = new System.Drawing.Point(13, 15);
            this.listConstants.Name = "listConstants";
            this.listConstants.Size = new System.Drawing.Size(120, 199);
            this.listConstants.TabIndex = 12;
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(197, 109);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(37, 20);
            this.txtFrom.TabIndex = 26;
            // 
            // txtStep
            // 
            this.txtStep.Location = new System.Drawing.Point(240, 109);
            this.txtStep.Name = "txtStep";
            this.txtStep.Size = new System.Drawing.Size(37, 20);
            this.txtStep.TabIndex = 27;
            this.txtStep.Visible = false;
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(283, 109);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(37, 20);
            this.txtTo.TabIndex = 28;
            this.txtTo.Visible = false;
            // 
            // ucConstants
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtTo);
            this.Controls.Add(this.txtStep);
            this.Controls.Add(this.txtFrom);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.chkMultipleValue);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listConstants);
            this.Name = "ucConstants";
            this.Size = new System.Drawing.Size(364, 228);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listConstants;
        private System.Windows.Forms.CheckBox chkMultipleValue;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.TextBox txtStep;
        private System.Windows.Forms.TextBox txtTo;
    }
}

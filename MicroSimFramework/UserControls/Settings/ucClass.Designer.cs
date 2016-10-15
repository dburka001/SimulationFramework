namespace MicroSimFramework
{
    partial class ucClass
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
            this.listProperties = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDefaultValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboxDefaultFields = new System.Windows.Forms.ComboBox();
            this.cboxTypes = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listProperties
            // 
            this.listProperties.FormattingEnabled = true;
            this.listProperties.Location = new System.Drawing.Point(25, 14);
            this.listProperties.Name = "listProperties";
            this.listProperties.Size = new System.Drawing.Size(120, 199);
            this.listProperties.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(168, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(209, 50);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // txtDefaultValue
            // 
            this.txtDefaultValue.Location = new System.Drawing.Point(209, 76);
            this.txtDefaultValue.Name = "txtDefaultValue";
            this.txtDefaultValue.Size = new System.Drawing.Size(100, 20);
            this.txtDefaultValue.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(168, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(168, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "label4";
            // 
            // cboxDefaultFields
            // 
            this.cboxDefaultFields.FormattingEnabled = true;
            this.cboxDefaultFields.Location = new System.Drawing.Point(210, 105);
            this.cboxDefaultFields.Name = "cboxDefaultFields";
            this.cboxDefaultFields.Size = new System.Drawing.Size(99, 21);
            this.cboxDefaultFields.TabIndex = 8;
            // 
            // cboxTypes
            // 
            this.cboxTypes.FormattingEnabled = true;
            this.cboxTypes.Location = new System.Drawing.Point(209, 18);
            this.cboxTypes.Name = "cboxTypes";
            this.cboxTypes.Size = new System.Drawing.Size(121, 21);
            this.cboxTypes.TabIndex = 9;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(171, 141);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 39);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(252, 141);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 39);
            this.btnRemove.TabIndex = 11;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // ucClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cboxTypes);
            this.Controls.Add(this.cboxDefaultFields);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDefaultValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listProperties);
            this.Name = "ucClass";
            this.Size = new System.Drawing.Size(338, 282);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listProperties;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDefaultValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboxDefaultFields;
        private System.Windows.Forms.ComboBox cboxTypes;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
    }
}

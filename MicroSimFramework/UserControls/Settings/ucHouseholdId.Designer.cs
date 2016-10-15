namespace MicroSimFramework
{
    partial class ucHouseholdId
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
            this.listHouseholdId = new System.Windows.Forms.ListBox();
            this.cboxFields = new System.Windows.Forms.ComboBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listHouseholdId
            // 
            this.listHouseholdId.FormattingEnabled = true;
            this.listHouseholdId.Location = new System.Drawing.Point(17, 12);
            this.listHouseholdId.Name = "listHouseholdId";
            this.listHouseholdId.Size = new System.Drawing.Size(154, 160);
            this.listHouseholdId.TabIndex = 0;
            // 
            // cboxFields
            // 
            this.cboxFields.FormattingEnabled = true;
            this.cboxFields.Location = new System.Drawing.Point(16, 178);
            this.cboxFields.Name = "cboxFields";
            this.cboxFields.Size = new System.Drawing.Size(155, 21);
            this.cboxFields.TabIndex = 1;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(96, 205);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 39);
            this.btnRemove.TabIndex = 13;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(15, 205);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 39);
            this.btnAdd.TabIndex = 12;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ucHouseholdId
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cboxFields);
            this.Controls.Add(this.listHouseholdId);
            this.Name = "ucHouseholdId";
            this.Size = new System.Drawing.Size(189, 257);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listHouseholdId;
        private System.Windows.Forms.ComboBox cboxFields;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
    }
}

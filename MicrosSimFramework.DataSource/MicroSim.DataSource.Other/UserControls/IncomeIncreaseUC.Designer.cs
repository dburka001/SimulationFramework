namespace MicroSim.DataSource.Other
{
    partial class IncomeIncreaseUC
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
            this.btnPercentages = new System.Windows.Forms.Button();
            this.btnIncrements = new System.Windows.Forms.Button();
            this.btnIncomes = new System.Windows.Forms.Button();
            this.btnIncomeTotals = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(500, 400);
            this.mainPanel.TabIndex = 0;
            // 
            // btnPercentages
            // 
            this.btnPercentages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPercentages.Location = new System.Drawing.Point(0, 400);
            this.btnPercentages.Name = "btnPercentages";
            this.btnPercentages.Size = new System.Drawing.Size(100, 100);
            this.btnPercentages.TabIndex = 1;
            this.btnPercentages.Tag = "1";
            this.btnPercentages.Text = "button1";
            this.btnPercentages.UseVisualStyleBackColor = true;
            this.btnPercentages.Click += new System.EventHandler(this.btnPercentagesClick);
            // 
            // btnIncrements
            // 
            this.btnIncrements.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIncrements.Location = new System.Drawing.Point(100, 400);
            this.btnIncrements.Name = "btnIncrements";
            this.btnIncrements.Size = new System.Drawing.Size(100, 100);
            this.btnIncrements.TabIndex = 2;
            this.btnIncrements.Tag = "2";
            this.btnIncrements.Text = "button1";
            this.btnIncrements.UseVisualStyleBackColor = true;
            this.btnIncrements.Click += new System.EventHandler(this.btnIncrementsClick);
            // 
            // btnIncomes
            // 
            this.btnIncomes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIncomes.Location = new System.Drawing.Point(200, 400);
            this.btnIncomes.Name = "btnIncomes";
            this.btnIncomes.Size = new System.Drawing.Size(100, 100);
            this.btnIncomes.TabIndex = 3;
            this.btnIncomes.Tag = "3";
            this.btnIncomes.Text = "button1";
            this.btnIncomes.UseVisualStyleBackColor = true;
            this.btnIncomes.Click += new System.EventHandler(this.btnIncomesClick);
            // 
            // btnIncomeTotals
            // 
            this.btnIncomeTotals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIncomeTotals.Location = new System.Drawing.Point(300, 400);
            this.btnIncomeTotals.Name = "btnIncomeTotals";
            this.btnIncomeTotals.Size = new System.Drawing.Size(100, 100);
            this.btnIncomeTotals.TabIndex = 4;
            this.btnIncomeTotals.Tag = "4";
            this.btnIncomeTotals.Text = "button1";
            this.btnIncomeTotals.UseVisualStyleBackColor = true;
            this.btnIncomeTotals.Click += new System.EventHandler(this.btnIncomeTotalsClick);
            // 
            // IncomeIncreaseUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.btnIncomeTotals);
            this.Controls.Add(this.btnIncomes);
            this.Controls.Add(this.btnIncrements);
            this.Controls.Add(this.btnPercentages);
            this.Controls.Add(this.mainPanel);
            this.Name = "IncomeIncreaseUC";
            this.Size = new System.Drawing.Size(500, 500);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Button btnPercentages;
        private System.Windows.Forms.Button btnIncrements;
        private System.Windows.Forms.Button btnIncomes;
        private System.Windows.Forms.Button btnIncomeTotals;
    }
}

namespace client_app
{
    partial class BudgetSchedulesControl
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
            this.groupBoxBudgetRules = new System.Windows.Forms.GroupBox();
            this.btnAddRule = new System.Windows.Forms.Button();
            this.dgvBudgetRules = new System.Windows.Forms.DataGridView();
            this.groupBoxBudgetRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBudgetRules)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxBudgetRules
            // 
            this.groupBoxBudgetRules.Controls.Add(this.btnAddRule);
            this.groupBoxBudgetRules.Controls.Add(this.dgvBudgetRules);
            this.groupBoxBudgetRules.Location = new System.Drawing.Point(36, 28);
            this.groupBoxBudgetRules.Name = "groupBoxBudgetRules";
            this.groupBoxBudgetRules.Size = new System.Drawing.Size(1149, 621);
            this.groupBoxBudgetRules.TabIndex = 0;
            this.groupBoxBudgetRules.TabStop = false;
            this.groupBoxBudgetRules.Text = "Budget Rules";
            // 
            // btnAddRule
            // 
            this.btnAddRule.Location = new System.Drawing.Point(981, 0);
            this.btnAddRule.Name = "btnAddRule";
            this.btnAddRule.Size = new System.Drawing.Size(93, 35);
            this.btnAddRule.TabIndex = 1;
            this.btnAddRule.Text = "Add Rule";
            this.btnAddRule.UseVisualStyleBackColor = true;
            // 
            // dgvBudgetRules
            // 
            this.dgvBudgetRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBudgetRules.Location = new System.Drawing.Point(18, 41);
            this.dgvBudgetRules.Name = "dgvBudgetRules";
            this.dgvBudgetRules.RowHeadersWidth = 51;
            this.dgvBudgetRules.RowTemplate.Height = 24;
            this.dgvBudgetRules.Size = new System.Drawing.Size(1110, 542);
            this.dgvBudgetRules.TabIndex = 0;
            // 
            // BudgetSchedulesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxBudgetRules);
            this.Name = "BudgetSchedulesControl";
            this.Size = new System.Drawing.Size(1205, 684);
            this.groupBoxBudgetRules.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBudgetRules)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxBudgetRules;
        private System.Windows.Forms.DataGridView dgvBudgetRules;
        private System.Windows.Forms.Button btnAddRule;
    }
}

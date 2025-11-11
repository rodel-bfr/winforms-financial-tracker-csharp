namespace client_app
{
    partial class Form1
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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dasboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTransactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.budgetRulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeCategoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 28);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1182, 725);
            this.mainPanel.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dasboardToolStripMenuItem,
            this.addTransactionToolStripMenuItem,
            this.budgetRulesToolStripMenuItem,
            this.customizeCategoriesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1182, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dasboardToolStripMenuItem
            // 
            this.dasboardToolStripMenuItem.Name = "dasboardToolStripMenuItem";
            this.dasboardToolStripMenuItem.Size = new System.Drawing.Size(88, 24);
            this.dasboardToolStripMenuItem.Text = "Dasboard";
            this.dasboardToolStripMenuItem.Click += new System.EventHandler(this.DashboardToolStripMenuItem_Click);
            // 
            // addTransactionToolStripMenuItem
            // 
            this.addTransactionToolStripMenuItem.Name = "addTransactionToolStripMenuItem";
            this.addTransactionToolStripMenuItem.Size = new System.Drawing.Size(130, 24);
            this.addTransactionToolStripMenuItem.Text = "Add Transaction";
            this.addTransactionToolStripMenuItem.Click += new System.EventHandler(this.AddTransactionToolStripMenuItem_Click);
            // 
            // budgetRulesToolStripMenuItem
            // 
            this.budgetRulesToolStripMenuItem.Name = "budgetRulesToolStripMenuItem";
            this.budgetRulesToolStripMenuItem.Size = new System.Drawing.Size(110, 24);
            this.budgetRulesToolStripMenuItem.Text = "Budget Rules";
            this.budgetRulesToolStripMenuItem.Click += new System.EventHandler(this.BudgetSchedulesToolStripMenuItem_Click);
            // 
            // customizeCategoriesToolStripMenuItem
            // 
            this.customizeCategoriesToolStripMenuItem.Name = "customizeCategoriesToolStripMenuItem";
            this.customizeCategoriesToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.customizeCategoriesToolStripMenuItem.Text = "Customize Categories";
            this.customizeCategoriesToolStripMenuItem.Click += new System.EventHandler(this.CustomizeCategoriesToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 753);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dasboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addTransactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem budgetRulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customizeCategoriesToolStripMenuItem;
    }
}


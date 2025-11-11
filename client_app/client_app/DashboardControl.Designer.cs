namespace client_app
{
    partial class DashboardControl
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.progressSavings = new System.Windows.Forms.ProgressBar();
            this.lblSavingsSpending = new System.Windows.Forms.Label();
            this.lblSavingsTitle = new System.Windows.Forms.Label();
            this.progressWants = new System.Windows.Forms.ProgressBar();
            this.lblWantsSpending = new System.Windows.Forms.Label();
            this.lblWantsTitle = new System.Windows.Forms.Label();
            this.progressNeeds = new System.Windows.Forms.ProgressBar();
            this.lblNeedsSpending = new System.Windows.Forms.Label();
            this.lblNeedsTitle = new System.Windows.Forms.Label();
            this.lblActiveRule = new System.Windows.Forms.Label();
            this.lblPeriodIncome = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chartSpending = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radMonthly = new System.Windows.Forms.RadioButton();
            this.radYearly = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTotalSavings = new System.Windows.Forms.TextBox();
            this.txtSavingsMade = new System.Windows.Forms.TextBox();
            this.txtTotalSpent = new System.Windows.Forms.TextBox();
            this.txtCashBalance = new System.Windows.Forms.TextBox();
            this.lblTotalSavings = new System.Windows.Forms.Label();
            this.lblSavingsMade = new System.Windows.Forms.Label();
            this.lblTotalSpent = new System.Windows.Forms.Label();
            this.lblCashBalance = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvTransactions = new System.Windows.Forms.DataGridView();
            this.btnResetDateFilter = new System.Windows.Forms.Button();
            this.btnApplyDateFilter = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSpending)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.progressSavings);
            this.groupBox5.Controls.Add(this.lblSavingsSpending);
            this.groupBox5.Controls.Add(this.lblSavingsTitle);
            this.groupBox5.Controls.Add(this.progressWants);
            this.groupBox5.Controls.Add(this.lblWantsSpending);
            this.groupBox5.Controls.Add(this.lblWantsTitle);
            this.groupBox5.Controls.Add(this.progressNeeds);
            this.groupBox5.Controls.Add(this.lblNeedsSpending);
            this.groupBox5.Controls.Add(this.lblNeedsTitle);
            this.groupBox5.Controls.Add(this.lblActiveRule);
            this.groupBox5.Controls.Add(this.lblPeriodIncome);
            this.groupBox5.Location = new System.Drawing.Point(698, 29);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(471, 269);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Income Allocation";
            // 
            // progressSavings
            // 
            this.progressSavings.Location = new System.Drawing.Point(9, 226);
            this.progressSavings.Name = "progressSavings";
            this.progressSavings.Size = new System.Drawing.Size(445, 23);
            this.progressSavings.TabIndex = 10;
            // 
            // lblSavingsSpending
            // 
            this.lblSavingsSpending.AutoSize = true;
            this.lblSavingsSpending.Location = new System.Drawing.Point(249, 206);
            this.lblSavingsSpending.Name = "lblSavingsSpending";
            this.lblSavingsSpending.Size = new System.Drawing.Size(128, 16);
            this.lblSavingsSpending.TabIndex = 9;
            this.lblSavingsSpending.Text = "lblSavingsSpending";
            // 
            // lblSavingsTitle
            // 
            this.lblSavingsTitle.AutoSize = true;
            this.lblSavingsTitle.Location = new System.Drawing.Point(6, 206);
            this.lblSavingsTitle.Name = "lblSavingsTitle";
            this.lblSavingsTitle.Size = new System.Drawing.Size(96, 16);
            this.lblSavingsTitle.TabIndex = 8;
            this.lblSavingsTitle.Text = "lblSavingsTitle";
            // 
            // progressWants
            // 
            this.progressWants.Location = new System.Drawing.Point(9, 160);
            this.progressWants.Name = "progressWants";
            this.progressWants.Size = new System.Drawing.Size(445, 23);
            this.progressWants.TabIndex = 7;
            // 
            // lblWantsSpending
            // 
            this.lblWantsSpending.AutoSize = true;
            this.lblWantsSpending.Location = new System.Drawing.Point(249, 141);
            this.lblWantsSpending.Name = "lblWantsSpending";
            this.lblWantsSpending.Size = new System.Drawing.Size(117, 16);
            this.lblWantsSpending.TabIndex = 6;
            this.lblWantsSpending.Text = "lblWantsSpending";
            // 
            // lblWantsTitle
            // 
            this.lblWantsTitle.AutoSize = true;
            this.lblWantsTitle.Location = new System.Drawing.Point(6, 140);
            this.lblWantsTitle.Name = "lblWantsTitle";
            this.lblWantsTitle.Size = new System.Drawing.Size(85, 16);
            this.lblWantsTitle.TabIndex = 5;
            this.lblWantsTitle.Text = "lblWantsTitle";
            // 
            // progressNeeds
            // 
            this.progressNeeds.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.progressNeeds.Location = new System.Drawing.Point(9, 104);
            this.progressNeeds.Name = "progressNeeds";
            this.progressNeeds.Size = new System.Drawing.Size(445, 23);
            this.progressNeeds.TabIndex = 4;
            // 
            // lblNeedsSpending
            // 
            this.lblNeedsSpending.AutoSize = true;
            this.lblNeedsSpending.Location = new System.Drawing.Point(249, 85);
            this.lblNeedsSpending.Name = "lblNeedsSpending";
            this.lblNeedsSpending.Size = new System.Drawing.Size(120, 16);
            this.lblNeedsSpending.TabIndex = 3;
            this.lblNeedsSpending.Text = "lblNeedsSpending";
            // 
            // lblNeedsTitle
            // 
            this.lblNeedsTitle.AutoSize = true;
            this.lblNeedsTitle.Location = new System.Drawing.Point(6, 85);
            this.lblNeedsTitle.Name = "lblNeedsTitle";
            this.lblNeedsTitle.Size = new System.Drawing.Size(88, 16);
            this.lblNeedsTitle.TabIndex = 2;
            this.lblNeedsTitle.Text = "lblNeedsTitle";
            // 
            // lblActiveRule
            // 
            this.lblActiveRule.AutoSize = true;
            this.lblActiveRule.Location = new System.Drawing.Point(8, 33);
            this.lblActiveRule.Name = "lblActiveRule";
            this.lblActiveRule.Size = new System.Drawing.Size(86, 16);
            this.lblActiveRule.TabIndex = 1;
            this.lblActiveRule.Text = "lblActiveRule";
            // 
            // lblPeriodIncome
            // 
            this.lblPeriodIncome.AutoSize = true;
            this.lblPeriodIncome.Location = new System.Drawing.Point(8, 63);
            this.lblPeriodIncome.Name = "lblPeriodIncome";
            this.lblPeriodIncome.Size = new System.Drawing.Size(105, 16);
            this.lblPeriodIncome.TabIndex = 0;
            this.lblPeriodIncome.Text = "lblPeriodIncome";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chartSpending);
            this.groupBox3.Location = new System.Drawing.Point(342, 29);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(316, 269);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Spending Distribution";
            // 
            // chartSpending
            // 
            chartArea3.Name = "ChartArea1";
            this.chartSpending.ChartAreas.Add(chartArea3);
            this.chartSpending.Location = new System.Drawing.Point(7, 22);
            this.chartSpending.Name = "chartSpending";
            this.chartSpending.Size = new System.Drawing.Size(303, 241);
            this.chartSpending.TabIndex = 0;
            this.chartSpending.Text = "chart1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radMonthly);
            this.groupBox1.Controls.Add(this.radYearly);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtTotalSavings);
            this.groupBox1.Controls.Add(this.txtSavingsMade);
            this.groupBox1.Controls.Add(this.txtTotalSpent);
            this.groupBox1.Controls.Add(this.txtCashBalance);
            this.groupBox1.Controls.Add(this.lblTotalSavings);
            this.groupBox1.Controls.Add(this.lblSavingsMade);
            this.groupBox1.Controls.Add(this.lblTotalSpent);
            this.groupBox1.Controls.Add(this.lblCashBalance);
            this.groupBox1.Controls.Add(this.cmbMonth);
            this.groupBox1.Controls.Add(this.cmbYear);
            this.groupBox1.Location = new System.Drawing.Point(24, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(281, 269);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Summary by Period";
            // 
            // radMonthly
            // 
            this.radMonthly.AutoSize = true;
            this.radMonthly.Checked = true;
            this.radMonthly.Location = new System.Drawing.Point(137, 31);
            this.radMonthly.Name = "radMonthly";
            this.radMonthly.Size = new System.Drawing.Size(67, 20);
            this.radMonthly.TabIndex = 16;
            this.radMonthly.TabStop = true;
            this.radMonthly.Text = "Mothly";
            this.radMonthly.UseVisualStyleBackColor = true;
            // 
            // radYearly
            // 
            this.radYearly.AutoSize = true;
            this.radYearly.Location = new System.Drawing.Point(13, 31);
            this.radYearly.Name = "radYearly";
            this.radYearly.Size = new System.Drawing.Size(67, 20);
            this.radYearly.TabIndex = 15;
            this.radYearly.Text = "Yearly";
            this.radYearly.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "Month";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Year";
            // 
            // txtTotalSavings
            // 
            this.txtTotalSavings.Location = new System.Drawing.Point(137, 228);
            this.txtTotalSavings.Name = "txtTotalSavings";
            this.txtTotalSavings.ReadOnly = true;
            this.txtTotalSavings.Size = new System.Drawing.Size(100, 22);
            this.txtTotalSavings.TabIndex = 11;
            // 
            // txtSavingsMade
            // 
            this.txtSavingsMade.Location = new System.Drawing.Point(137, 199);
            this.txtSavingsMade.Name = "txtSavingsMade";
            this.txtSavingsMade.ReadOnly = true;
            this.txtSavingsMade.Size = new System.Drawing.Size(100, 22);
            this.txtSavingsMade.TabIndex = 10;
            // 
            // txtTotalSpent
            // 
            this.txtTotalSpent.Location = new System.Drawing.Point(137, 170);
            this.txtTotalSpent.Name = "txtTotalSpent";
            this.txtTotalSpent.ReadOnly = true;
            this.txtTotalSpent.Size = new System.Drawing.Size(100, 22);
            this.txtTotalSpent.TabIndex = 9;
            // 
            // txtCashBalance
            // 
            this.txtCashBalance.Location = new System.Drawing.Point(137, 141);
            this.txtCashBalance.Name = "txtCashBalance";
            this.txtCashBalance.ReadOnly = true;
            this.txtCashBalance.Size = new System.Drawing.Size(100, 22);
            this.txtCashBalance.TabIndex = 8;
            // 
            // lblTotalSavings
            // 
            this.lblTotalSavings.AutoSize = true;
            this.lblTotalSavings.Location = new System.Drawing.Point(10, 234);
            this.lblTotalSavings.Name = "lblTotalSavings";
            this.lblTotalSavings.Size = new System.Drawing.Size(90, 16);
            this.lblTotalSavings.TabIndex = 7;
            this.lblTotalSavings.Text = "Total Savings";
            // 
            // lblSavingsMade
            // 
            this.lblSavingsMade.AutoSize = true;
            this.lblSavingsMade.Location = new System.Drawing.Point(10, 205);
            this.lblSavingsMade.Name = "lblSavingsMade";
            this.lblSavingsMade.Size = new System.Drawing.Size(94, 16);
            this.lblSavingsMade.TabIndex = 6;
            this.lblSavingsMade.Text = "Savings Made";
            // 
            // lblTotalSpent
            // 
            this.lblTotalSpent.AutoSize = true;
            this.lblTotalSpent.Location = new System.Drawing.Point(10, 176);
            this.lblTotalSpent.Name = "lblTotalSpent";
            this.lblTotalSpent.Size = new System.Drawing.Size(76, 16);
            this.lblTotalSpent.TabIndex = 5;
            this.lblTotalSpent.Text = "Total Spent";
            // 
            // lblCashBalance
            // 
            this.lblCashBalance.AutoSize = true;
            this.lblCashBalance.Location = new System.Drawing.Point(10, 147);
            this.lblCashBalance.Name = "lblCashBalance";
            this.lblCashBalance.Size = new System.Drawing.Size(91, 16);
            this.lblCashBalance.TabIndex = 4;
            this.lblCashBalance.Text = "Cash Balance";
            // 
            // cmbMonth
            // 
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(137, 96);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(121, 24);
            this.cmbMonth.TabIndex = 3;
            // 
            // cmbYear
            // 
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(137, 63);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(121, 24);
            this.cmbYear.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvTransactions);
            this.groupBox2.Controls.Add(this.btnResetDateFilter);
            this.groupBox2.Controls.Add(this.btnApplyDateFilter);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dtpTo);
            this.groupBox2.Controls.Add(this.dtpFrom);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(24, 322);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1145, 390);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Transaction History";
            // 
            // dgvTransactions
            // 
            this.dgvTransactions.AllowUserToAddRows = false;
            this.dgvTransactions.AllowUserToDeleteRows = false;
            this.dgvTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransactions.Location = new System.Drawing.Point(19, 38);
            this.dgvTransactions.Name = "dgvTransactions";
            this.dgvTransactions.RowHeadersWidth = 51;
            this.dgvTransactions.RowTemplate.Height = 24;
            this.dgvTransactions.Size = new System.Drawing.Size(1109, 332);
            this.dgvTransactions.TabIndex = 10;
            // 
            // btnResetDateFilter
            // 
            this.btnResetDateFilter.Location = new System.Drawing.Point(826, 3);
            this.btnResetDateFilter.Name = "btnResetDateFilter";
            this.btnResetDateFilter.Size = new System.Drawing.Size(75, 33);
            this.btnResetDateFilter.TabIndex = 6;
            this.btnResetDateFilter.Text = "Reset";
            this.btnResetDateFilter.UseVisualStyleBackColor = true;
            // 
            // btnApplyDateFilter
            // 
            this.btnApplyDateFilter.Location = new System.Drawing.Point(745, 3);
            this.btnApplyDateFilter.Name = "btnApplyDateFilter";
            this.btnApplyDateFilter.Size = new System.Drawing.Size(75, 33);
            this.btnApplyDateFilter.TabIndex = 5;
            this.btnApplyDateFilter.Text = "Apply";
            this.btnApplyDateFilter.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(493, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "To:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(230, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "From:";
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(526, 11);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(121, 22);
            this.dtpTo.TabIndex = 2;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(277, 11);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(121, 22);
            this.dtpFrom.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(138, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "FIlter by Date";
            // 
            // DashboardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "DashboardControl";
            this.Size = new System.Drawing.Size(1200, 900);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartSpending)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ProgressBar progressSavings;
        private System.Windows.Forms.Label lblSavingsSpending;
        private System.Windows.Forms.Label lblSavingsTitle;
        private System.Windows.Forms.ProgressBar progressWants;
        private System.Windows.Forms.Label lblWantsSpending;
        private System.Windows.Forms.Label lblWantsTitle;
        private System.Windows.Forms.ProgressBar progressNeeds;
        private System.Windows.Forms.Label lblNeedsSpending;
        private System.Windows.Forms.Label lblNeedsTitle;
        private System.Windows.Forms.Label lblActiveRule;
        private System.Windows.Forms.Label lblPeriodIncome;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSpending;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radMonthly;
        private System.Windows.Forms.RadioButton radYearly;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTotalSavings;
        private System.Windows.Forms.TextBox txtSavingsMade;
        private System.Windows.Forms.TextBox txtTotalSpent;
        private System.Windows.Forms.TextBox txtCashBalance;
        private System.Windows.Forms.Label lblTotalSavings;
        private System.Windows.Forms.Label lblSavingsMade;
        private System.Windows.Forms.Label lblTotalSpent;
        private System.Windows.Forms.Label lblCashBalance;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvTransactions;
        private System.Windows.Forms.Button btnResetDateFilter;
        private System.Windows.Forms.Button btnApplyDateFilter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label1;
    }
}

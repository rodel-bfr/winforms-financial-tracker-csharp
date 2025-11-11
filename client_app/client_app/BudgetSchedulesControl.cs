// Bringing in the usual toolkits.
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using client_app.ServiceReference1;

namespace client_app
{
    /// <summary>
    /// This is my 'page' for managing budget rules, like the 50/30/20 rule.
    /// It's a UserControl, so I can just drop it into my main form.
    /// Its only job is to show, add, edit, and delete budget rules.
    /// </summary>
    public partial class BudgetSchedulesControl : UserControl
    {
        // This is my "call home" event, just like on the Dashboard.
        // When I make a change here, I'll fire this to tell Form1 to reload everything.
        public event EventHandler DataChanged;
        // My connection to the server.
        private readonly WebService1SoapClient service = new WebService1SoapClient();

        #region Member Variables
        // This is the local list of all my budget rules, passed in from Form1.
        private List<BudgetRule> allBudgetRules;
        #endregion

        public BudgetSchedulesControl()
        {
            InitializeComponent();
            SetupBudgetGrid(); // Set up the columns and styles for my grid.
            WireUpEventHandlers(); // Connect my buttons to their click methods.
        }

        /// <summary>
        /// The main entry point for this control. Form1 calls this to give me the data I need.
        /// </summary>
        public void LoadData(List<BudgetRule> budgetRules)
        {
            this.allBudgetRules = budgetRules; // Store the list locally.
            RefreshBudgetGrid(); // Fill the grid with the rules.
        }

        #region Grid & Control Setup
        /// <summary>
        /// This method sets up the appearance of my DataGridView for budget rules.
        /// It only runs once when the control is created.
        /// </summary>
        private void SetupBudgetGrid()
        {
            dgvBudgetRules.RowHeadersVisible = false; // I don't need the blank column on the left.
            dgvBudgetRules.AllowUserToAddRows = false; // The user should only add rows via the button.
            dgvBudgetRules.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Click anywhere on a row to select it.
            dgvBudgetRules.Columns.Clear();
            dgvBudgetRules.AutoGenerateColumns = false; // I want to define the columns myself.

            // Manually add each column. The 'Fill' one will expand to take up extra space.
            dgvBudgetRules.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Rule Name", Name = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvBudgetRules.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ratios (N/W/S)", Name = "Ratios", Width = 120 });
            dgvBudgetRules.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "From", Name = "From", Width = 110 });
            dgvBudgetRules.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "To", Name = "To", Width = 110 });
            dgvBudgetRules.Columns.Add(new DataGridViewButtonColumn { HeaderText = "Edit", Text = "Edit", UseColumnTextForButtonValue = true, Name = "Edit", Width = 60 });
            dgvBudgetRules.Columns.Add(new DataGridViewButtonColumn { HeaderText = "Delete", Text = "Delete", UseColumnTextForButtonValue = true, Name = "Delete", Width = 60 });
        }

        /// <summary>
        /// Connects my UI elements to the methods that should run when they are interacted with.
        /// </summary>
        private void WireUpEventHandlers()
        {
            btnAddRule.Click += BtnAddRule_Click;
            dgvBudgetRules.CellContentClick += DgvBudgetRules_CellContentClick;
        }
        #endregion

        #region Grid Refresh Logic
        /// <summary>
        /// This method clears and re-populates the grid with the current list of budget rules.
        /// I call this every time the data is loaded.
        /// </summary>
        private void RefreshBudgetGrid()
        {
            dgvBudgetRules.Rows.Clear();
            if (allBudgetRules == null) return; // Safety check.

            foreach (var rule in allBudgetRules)
            {
                // Format the ratios to look nice, e.g., "50% / 30% / 20%".
                // The ':0.#' format specifier gets rid of trailing zeros (e.g., 50.0 becomes 50).
                string ratios = $"{rule.needs_ratio:0.#}% / {rule.wants_ratio:0.#}% / {rule.savings_ratio:0.#}%";
                // If the end date is null, show "Ongoing" to the user.
                string endDate = rule.end_date.HasValue ? rule.end_date.Value.ToShortDateString() : "Ongoing";

                // Add the new row with my nicely formatted strings.
                int rowIndex = dgvBudgetRules.Rows.Add(rule.name, ratios, rule.start_date.ToShortDateString(), endDate);

                // REMINDER: Storing the full object in the .Tag property is the key to making the Edit/Delete buttons work easily.
                dgvBudgetRules.Rows[rowIndex].Tag = rule;
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Runs when the "Add New Rule" button is clicked.
        /// </summary>
        private async void BtnAddRule_Click(object sender, EventArgs e)
        {
            // Open the BudgetRuleDialog in "add" mode by passing 'null' to the constructor.
            using (var dialog = new BudgetRuleDialog(null))
            {
                // If I click "Save" on the dialog...
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Get the new rule object that the dialog created.
                        var newRule = dialog.Rule;
                        // Call the server to add it to the database.
                        await service.AddBudgetRuleAsync(newRule.name, newRule.start_date, newRule.end_date, newRule.needs_ratio, newRule.wants_ratio, newRule.savings_ratio);
                        // Fire the event to tell Form1 to reload all data.
                        DataChanged?.Invoke(this, EventArgs.Empty);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to add rule. Error: {ex.Message}", "API Error");
                    }
                }
            }
        }

        /// <summary>
        /// Runs when I click on a button inside the grid (Edit or Delete).
        /// </summary>
        private async void DgvBudgetRules_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignore clicks on the header row.

            // Get the full BudgetRule object from the row's Tag property.
            var rule = dgvBudgetRules.Rows[e.RowIndex].Tag as BudgetRule;
            if (rule == null) return;

            try
            {
                // Check which column the clicked button was in.
                if (dgvBudgetRules.Columns[e.ColumnIndex].Name == "Edit")
                {
                    // Open the dialog in "edit" mode by passing the existing rule to the constructor.
                    using (var dialog = new BudgetRuleDialog(rule))
                    {
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            var updatedRule = dialog.Rule;
                            await service.UpdateBudgetRuleAsync(updatedRule.id, updatedRule.name, updatedRule.start_date, updatedRule.end_date, updatedRule.needs_ratio, updatedRule.wants_ratio, updatedRule.savings_ratio);
                            // Tell Form1 to reload everything.
                            DataChanged?.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
                else if (dgvBudgetRules.Columns[e.ColumnIndex].Name == "Delete")
                {
                    // Always confirm before deleting data!
                    if (MessageBox.Show($"Are you sure you want to delete the rule '{rule.name}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        await service.DeleteBudgetRuleAsync(rule.id);
                        // Tell Form1 to reload everything.
                        DataChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to manage the budget rule. Error: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}

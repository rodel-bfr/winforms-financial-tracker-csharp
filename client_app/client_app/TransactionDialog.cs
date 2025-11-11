// Bringing in my standard toolkits. LINQ is always useful for filtering lists.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using client_app.ServiceReference1;

namespace client_app
{
    /// <summary>
    /// This is my pop-up window for adding or editing a transaction.
    /// It's a self-contained form that I'll show using 'ShowDialog()'.
    /// </summary>
    public partial class TransactionDialog : Form
    {
        // This is the most important property. After the user clicks "Save",
        // this object will hold all the info for the new or updated transaction.
        // Form1 will then grab this object to send its data to the server.
        public Transaction ResultTransaction { get; private set; }

        // Local copy of all the categories, passed in from Form1. I need this for the dropdown list.
        private readonly List<Category> allCategories;
        // A simple flag to track if I'm adding a new transaction or editing an existing one.
        private readonly bool isEditMode;

        /// <summary>
        /// This is the constructor. It's the method that runs when I create a new TransactionDialog.
        /// The 'transactionToEdit = null' part makes that parameter optional.
        /// If I pass in a transaction, it's edit mode. If I don't, it's add mode.
        /// </summary>
        public TransactionDialog(List<Category> categories, Transaction transactionToEdit = null)
        {
            InitializeComponent();

            this.allCategories = categories;
            // The 'isEditMode' flag is set based on whether 'transactionToEdit' was provided. Simple and effective.
            this.isEditMode = (transactionToEdit != null);

            // If I'm editing, use the transaction that was passed in.
            // If I'm adding, create a brand new, empty transaction object.
            // The '??' is the null-coalescing operator. It's a shortcut for the longer,
            // but easier-to-understand if-else block below. It means:
            // "If 'transactionToEdit' is not null, use it. Otherwise, use a new Transaction()."
            /*
                // This is the beginner-friendly version of the line below:
                if (transactionToEdit != null)
                {
                    this.ResultTransaction = transactionToEdit;
                }
                else
                {
                    this.ResultTransaction = new Transaction();
                }
            */
            this.ResultTransaction = transactionToEdit ?? new Transaction();

            // Hooking up my event handlers. This is how I make my buttons and dropdowns do things.
            this.Load += TransactionDialog_Load; // Runs when the dialog first opens.
            this.btnSave.Click += BtnSave_Click; // Runs when I click the save button.
            this.cmbType.SelectedIndexChanged += CmbType_SelectedIndexChanged; // Runs when I change the transaction type.
        }

        /// <summary>
        /// This code runs right when the dialog loads, before it's shown to the user.
        /// It's the perfect place to set up the form for either "add" or "edit" mode.
        /// </summary>
        private void TransactionDialog_Load(object sender, EventArgs e)
        {
            // --- REMINDER: This is a common and important trick! ---
            // I'm about to set the initial values of the form controls. I don't want the 'SelectedIndexChanged' event
            // to fire while I'm doing this setup. So, I temporarily "unhook" it.
            this.cmbType.SelectedIndexChanged -= CmbType_SelectedIndexChanged;

            if (isEditMode)
            {
                // Set up the form for editing.
                this.Text = "Edit Transaction";
                btnSave.Text = "Save Changes";
                // This method will fill in all the text boxes and dropdowns with the existing transaction's data.
                LoadTransactionData();
            }
            else
            {
                // Set up the form for adding a new transaction.
                this.Text = "Add New Transaction";
                btnSave.Text = "Add Transaction";
                dtpDate.Value = DateTime.Now; // Default the date to today.
                cmbType.SelectedItem = "Expense"; // Most transactions are expenses, so it's a good default.
            }

            // Now that the 'Type' dropdown has its initial value, I can manually call the method
            // to populate the 'Category' dropdown based on that type.
            PopulateCategoryComboBox();

            // If I'm in edit mode, now is the time to select the correct category in the (now populated) category list.
            if (isEditMode && ResultTransaction.category_id.HasValue)
            {
                // A safety check to make sure the category I'm trying to select actually exists in the dropdown.
                if (cmbCategory.Items.Cast<Category>().Any(c => c.id == ResultTransaction.category_id.Value))
                {
                    cmbCategory.SelectedValue = ResultTransaction.category_id.Value;
                }
            }

            // --- IMPORTANT: Remember to re-hook the event handler! ---
            // Now that setup is done, I can put the event handler back so it works when the user changes the type.
            this.cmbType.SelectedIndexChanged += CmbType_SelectedIndexChanged;
        }

        /// <summary>
        /// Fills the form's controls with the data from the transaction being edited.
        /// </summary>
        private void LoadTransactionData()
        {
            dtpDate.Value = ResultTransaction.transaction_date;
            txtDescription.Text = ResultTransaction.description;
            numAmount.Value = Math.Abs(ResultTransaction.amount); // I always show the amount as a positive number in the UI.

            // This logic translates my two API types ("Income", "Expense") into my four user-friendly UI types.
            var category = allCategories.FirstOrDefault(c => c.id == ResultTransaction.category_id);
            if (ResultTransaction.type == "Income")
            {
                cmbType.SelectedItem = (category?.type == "Savings") ? "Withdrawal from Savings" : "Income";
            }
            else // It must be an "Expense"
            {
                cmbType.SelectedItem = (category?.type == "Savings") ? "Transfer to Savings" : "Expense";
            }
        }

        // This is just a wrapper. When the user changes the type, it calls my main logic method.
        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateCategoryComboBox();
        }

        /// <summary>
        /// This is the logic that filters the 'Category' dropdown.
        /// For example, if "Income" is selected, it only shows income categories.
        /// </summary>
        private void PopulateCategoryComboBox()
        {
            if (allCategories is null || cmbType.SelectedItem is null) return;

            string selectedUiType = cmbType.SelectedItem.ToString();
            List<Category> filteredCategories;

            // A 'switch' statement is a clean way to handle the different filter logic for each type.
            switch (selectedUiType)
            {
                case "Income":
                    // Use LINQ to find all categories where the type is "Income".
                    filteredCategories = allCategories.Where(c => c.type != null && c.type.Trim().Equals("Income", StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                case "Expense":
                    // For a general expense, I want to show categories that are either "Needs" OR "Wants".
                    filteredCategories = allCategories.Where(c => c.type != null && (c.type.Trim().Equals("Needs", StringComparison.OrdinalIgnoreCase) || c.type.Trim().Equals("Wants", StringComparison.OrdinalIgnoreCase))).ToList();
                    break;

                case "Transfer to Savings":
                case "Withdrawal from Savings":
                    // For both savings-related types, I only want to show "Savings" categories.
                    filteredCategories = allCategories.Where(c => c.type != null && c.type.Trim().Equals("Savings", StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                default:
                    // If something goes wrong, just show an empty list.
                    filteredCategories = new List<Category>();
                    break;
            }

            object currentSelection = cmbCategory.SelectedValue; // Remember what was selected before I change the list.

            // This is how I bind a list of objects to a ComboBox.
            cmbCategory.DataSource = filteredCategories; // The source of the items.
            cmbCategory.DisplayMember = "name"; // The property to SHOW to the user (e.g., "Salary").
            cmbCategory.ValueMember = "id"; // The hidden value behind each item (e.g., 1).

            // Try to re-select the item that was selected before, if it's still in the new filtered list.
            if (currentSelection != null && filteredCategories.Any(c => c.id == (int)currentSelection))
            {
                cmbCategory.SelectedValue = currentSelection;
            }
            // If nothing is selected, just default to the first item in the list.
            else if (cmbCategory.SelectedIndex == -1 && filteredCategories.Any())
            {
                cmbCategory.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// This runs when I click the save button. It validates the input, updates the
        /// 'ResultTransaction' object, and closes the dialog.
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Basic validation to make sure the user filled everything out.
            if (string.IsNullOrWhiteSpace(txtDescription.Text) || numAmount.Value <= 0 || cmbCategory.SelectedValue is null)
            {
                MessageBox.Show("Please fill out all fields correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop the method here if validation fails.
            }

            // Translate the user-friendly type from the UI back to the simple "Income" or "Expense" type that the server expects.
            string uiType = cmbType.SelectedItem.ToString();
            string apiType = (uiType == "Income" || uiType == "Withdrawal from Savings") ? "Income" : "Expense";

            // Update all the properties of my ResultTransaction object with the values from the form.
            ResultTransaction.description = txtDescription.Text;
            ResultTransaction.amount = numAmount.Value;
            ResultTransaction.type = apiType;
            ResultTransaction.category_id = (int)cmbCategory.SelectedValue;
            ResultTransaction.transaction_date = dtpDate.Value;

            // This is the important part. Setting DialogResult to OK tells Form1's 'ShowDialog()' call
            // that the user finished successfully. It also automatically closes the form.
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

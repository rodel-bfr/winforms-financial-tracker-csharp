// Just need the basics for this form.
using System;
using System.Windows.Forms;
using client_app.ServiceReference1;

namespace client_app
{
    /// <summary>
    /// This is my pop-up window for adding or editing a budget rule.
    /// It's a simple, focused form.
    /// </summary>
    public partial class BudgetRuleDialog : Form
    {
        // This public property will hold the final result.
        // After I click "Save", the BudgetSchedulesControl will grab this 'Rule' object
        // to get all the info it needs to send to the server.
        public BudgetRule Rule { get; private set; }

        /// <summary>
        /// This is my constructor. It's set up to handle both Add and Edit modes.
        /// If I pass in an existing rule, it edits. If I pass in null, it creates a new one.
        /// </summary>
        public BudgetRuleDialog(BudgetRule rule = null)
        {
            InitializeComponent();

            // This is a slick way to handle both add and edit modes in one line.
            // The '??' (null-coalescing operator) means:
            // "If 'rule' is not null, use it. Otherwise, create a new BudgetRule with these default values."
            /*
                // This is the beginner-friendly version of the line below:
                if (rule != null)
                {
                    // If we are editing, use the existing rule that was passed in.
                    this.Rule = rule;
                }
                else
                {
                    // If we are adding, create a brand new rule with some sensible defaults.
                    this.Rule = new BudgetRule 
                    { 
                        start_date = DateTime.Now, 
                        needs_ratio = 50, 
                        wants_ratio = 30, 
                        savings_ratio = 20 
                    };
                }
            */
            this.Rule = rule ?? new BudgetRule { start_date = DateTime.Now, needs_ratio = 50, wants_ratio = 30, savings_ratio = 20 };

            // Now that 'this.Rule' is guaranteed to have a value, I can load its data into the form.
            LoadRuleData();

            // Hook up my button click events. I'm using lambda expressions here for short, simple events.
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            chkEnableEndDate.CheckedChanged += (s, e) => { dtpEndDate.Enabled = chkEnableEndDate.Checked; };
        }

        /// <summary>
        /// This method takes the data from my 'Rule' object and uses it to fill in the text boxes,
        /// date pickers, and number spinners on the form.
        /// </summary>
        private void LoadRuleData()
        {
            txtName.Text = Rule.name;
            dtpStartDate.Value = Rule.start_date;

            // Handle the nullable end date.
            if (Rule.end_date.HasValue)
            {
                // If there's an end date, check the box and set the date picker's value.
                chkEnableEndDate.Checked = true;
                dtpEndDate.Value = Rule.end_date.Value;
            }
            else
            {
                // If there's no end date, uncheck the box and disable the date picker.
                chkEnableEndDate.Checked = false;
                dtpEndDate.Enabled = false;
            }

            // Set the values for the ratio spinners.
            numNeeds.Value = Rule.needs_ratio;
            numWants.Value = Rule.wants_ratio;
            numSavings.Value = Rule.savings_ratio;
        }

        /// <summary>
        /// This method runs when I click the "Save" button.
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // --- REMINDER: Always validate user input! ---
            // This is my most important validation rule for this form.
            if (numNeeds.Value + numWants.Value + numSavings.Value != 100)
            {
                MessageBox.Show("Ratios must add up to 100.", "Validation Error");
                return; // Stop the save process if the numbers are wrong.
            }

            // If validation passes, update my 'Rule' object with the new values from the form.
            Rule.name = txtName.Text;
            Rule.start_date = dtpStartDate.Value;

            // This is a ternary operator - a compact if/else statement.
            // It says: "If the checkbox is checked, set the end_date to the date picker's value. Otherwise, set it to null."
            Rule.end_date = chkEnableEndDate.Checked ? (DateTime?)dtpEndDate.Value : null;

            Rule.needs_ratio = numNeeds.Value;
            Rule.wants_ratio = numWants.Value;
            Rule.savings_ratio = numSavings.Value;

            // This tells the calling form (BudgetSchedulesControl) that I finished successfully.
            this.DialogResult = DialogResult.OK;
            // This closes the pop-up window.
            this.Close();
        }
    }
}

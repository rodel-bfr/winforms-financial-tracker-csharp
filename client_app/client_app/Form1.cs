// Pulling in the toolkits I need.
// System.Threading.Tasks is SUPER important for async/await, which keeps my app from freezing.
// The ServiceReference1 is the one Visual Studio generated for me so I can talk to my web service.
using System;
using System.Collections.Generic;   // For lists and dictionaries to hold my transactions, categories, and budget rules.
using System.Threading.Tasks;   // This is essential for the async/await pattern. It provides the Task object, which keeps the application responsive and prevents it from freezing while loading data from the server.
using System.Windows.Forms;
using client_app.ServiceReference1;

namespace client_app
{
    /// <summary>
    /// This is my main application window. It's the 'container' for everything else.
    /// Controls which view is shown.
    /// </summary>
    public partial class Form1 : Form
    {
        // This part is important to talk to the server. Visual Studio created this class for me
        // when I added the 'Service Reference'. It has all the methods from my server code.
        private readonly WebService1SoapClient service = new WebService1SoapClient();

        // --- Master Data Lists ---
        // These are the single source of truth for my whole app.
        // I'll load all the data from the server into these lists just once (per refresh).
        // Then, I'll pass these lists down to my other screens (User Controls).
        public List<Transaction> AllTransactions;
        public List<Category> AllCategories;
        public List<BudgetRule> AllBudgetRules;

        // --- User Controls (My "Views" or "Pages") ---
        // Instead of creating a ton of different forms, I have one main form
        // and I just swap these user controls in and out of a panel. It's much cleaner.
        private readonly DashboardControl dashboardView;
        private readonly CategoriesControl categoriesView;
        private readonly BudgetSchedulesControl budgetSchedulesView;

        public Form1()
        {
            InitializeComponent(); // Standard Windows Forms stuff, builds the UI from the designer.

            // I need to create an instance of each of my 'pages' so they're ready to be shown.
            dashboardView = new DashboardControl();
            categoriesView = new CategoriesControl();
            budgetSchedulesView = new BudgetSchedulesControl();

            // This is a key part of my design. I'm 'subscribing' this form to an event from each child control.
            // So, when the categoriesView says "Hey, data changed!", my OnDataChanged method will run automatically.
            // This lets the child controls be independent - they don't need to know about Form1 directly.
            dashboardView.DataChanged += OnDataChanged;
            categoriesView.DataChanged += OnDataChanged;
            budgetSchedulesView.DataChanged += OnDataChanged;
        }

        // This function runs automatically as soon as the app window loads for the first time.
        // It's the perfect place to get my initial data from the server.
        private async void Form1_Load(object sender, EventArgs e)
        {
            // I'm calling the main data loading method here.
            await ReloadAllDataAsync();
        }

        // This is my 'listener' method. It gets triggered by any of the User Controls
        // whenever they fire their 'DataChanged' event (e.g., after I add, update, or delete something).
        private async void OnDataChanged(object sender, EventArgs e)
        {
            // A change happened, so I need to reload everything to stay in sync.
            await ReloadAllDataAsync();
        }

        /// <summary>
        /// This is the BOSS method of this form. It does all the heavy lifting.
        /// It's marked 'async' so it can use 'await', which means the app's UI won't freeze while it's talking to the server.
        /// </summary>
        private async Task ReloadAllDataAsync()
        {
            // Give the user some feedback that something is happening.
            this.Text = "Financial Tracker - Loading...";
            this.mainPanel.Enabled = false; // Disable the main panel so I can't click anything while it's loading.

            try
            {
                // Here, I call the server to get all the data.
                // 'await' tells my app to "pause" this method here, go do other things (like keep the UI responsive),
                // and come back to the next line only when the server has responded.
                var transactionsResponse = await service.GetTransactionsAsync();
                var categoriesResponse = await service.GetCategoriesAsync();
                var budgetRulesResponse = await service.GetBudgetRulesAsync();

                // Now that I have the responses, I can fill up my master lists.
                // The actual list is inside the 'Body' and '...Result' properties of the response object.
                AllTransactions = new List<Transaction>(transactionsResponse.Body.GetTransactionsResult);
                AllCategories = new List<Category>(categoriesResponse.Body.GetCategoriesResult);
                AllBudgetRules = new List<BudgetRule>(budgetRulesResponse.Body.GetBudgetRulesResult);

                // This logic is to make sure the view I was on before the refresh is the view I see after.
                // If I was on the Categories screen, show the Categories screen again (but with the new data).
                if (mainPanel.Controls.Contains(categoriesView))
                {
                    ShowCategories();
                }
                else if (mainPanel.Controls.Contains(budgetSchedulesView))
                {
                    ShowBudgetSchedules();
                }
                else
                {
                    // If I'm not on any other specific screen, just default to the dashboard.
                    ShowDashboard();
                }
            }
            catch (Exception ex)
            {
                // If anything goes wrong (like the server is down), show a friendly error message.
                MessageBox.Show($"An error occurred while loading data: {ex.Message}", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Text = "Financial Tracker - Error";
            }
            finally
            {
                // This 'finally' block will run NO MATTER WHAT - whether the 'try' succeeded or the 'catch' was triggered.
                // It's the perfect place to re-enable the UI.
                this.Text = "Financial Tracker";
                this.mainPanel.Enabled = true;
            }
        }

        // --- View Switching Methods ---
        // The next three methods are simple screen-switchers.

        private void ShowDashboard()
        {
            if (this.AllTransactions == null) return; // Don't do anything if the data hasn't loaded yet.
            mainPanel.Controls.Clear(); // Empty the stage.
            dashboardView.Dock = DockStyle.Fill; // Make the user control fill the entire panel.
            mainPanel.Controls.Add(dashboardView); // Put the dashboard on the stage.
            dashboardView.LoadData(this.AllTransactions, this.AllCategories, this.AllBudgetRules); // Give it its script (the data).
        }

        private void ShowCategories()
        {
            if (this.AllCategories == null) return;
            mainPanel.Controls.Clear();
            categoriesView.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(categoriesView);
            categoriesView.LoadCategories(this.AllCategories); // This view only needs the categories list.
        }

        private void ShowBudgetSchedules()
        {
            if (this.AllBudgetRules == null) return;
            mainPanel.Controls.Clear();
            budgetSchedulesView.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(budgetSchedulesView);
            budgetSchedulesView.LoadData(this.AllBudgetRules); // This view only needs the budget rules list.
        }


        // --- Menu Item Click Handlers ---
        // These are the simple methods that run when I click on the menu items at the top of the form.

        private async void AddTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // A quick check to make sure the categories are loaded, because my transaction dialog needs them.
            if (this.AllCategories == null) { MessageBox.Show("Categories are not yet loaded."); return; }

            // Show the pop-up dialog for adding a new transaction.
            // The 'using' block ensures the dialog is disposed of properly afterwards.
            using (var dialog = new TransactionDialog(this.AllCategories, null))
            {
                // 'ShowDialog()' pauses the code here until the user closes the pop-up.
                // If I clicked the 'OK' button on the dialog...
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Get the new transaction object that the dialog created.
                        var newTransaction = dialog.ResultTransaction;
                        // Call the server to add it to the database.
                        await service.AddTransactionAsync(newTransaction.description, newTransaction.amount, newTransaction.type, newTransaction.category_id, newTransaction.transaction_date);
                        // Now that the data on the server has changed, call the BOSS to reload everything.
                        await ReloadAllDataAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to add transaction. Error: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // These are just one-liners that call the screen-switching methods.
        private void DashboardToolStripMenuItem_Click(object sender, EventArgs e) { ShowDashboard(); }
        private void BudgetSchedulesToolStripMenuItem_Click(object sender, EventArgs e) { ShowBudgetSchedules(); }
        private void CustomizeCategoriesToolStripMenuItem_Click(object sender, EventArgs e) { ShowCategories(); }
    }
}
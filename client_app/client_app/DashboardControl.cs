// Bringing in my usual toolkits.
// Linq is super important here for all my data filtering and calculations.
// The DataVisualization library is what lets me draw the pie chart.
using System;
using System.Collections.Generic;   // For lists and dictionaries to hold my transactions, categories, and budget rules.
using System.Data;  // For DataTable, which I use to bind data to the DataGridView.
using System.Drawing;   //For colors and drawing the custom Amount column in the transaction grid.
using System.Linq;  //For LINQ queries to filter and sort my transactions, categories, and budget rules.
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; //For the pie chart that shows spending by category.
using client_app.ServiceReference1;

namespace client_app
{
    /// <summary>
    /// This is my Dashboard 'page'. It's a UserControl, which is like a reusable mini-form
    /// that I can just plug into my main Form1. This is where most of my app's logic lives.
    /// </summary>
    public partial class DashboardControl : UserControl
    {
        // This is my "call home" event. When I edit or delete a transaction here,
        // I'll fire this event to tell Form1, "Hey, the data has changed, you need to reload everything from the server!"
        // This is a great way to keep my code clean. This control doesn't need to know HOW to reload, just THAT it needs to happen.
        public event EventHandler DataChanged;

        // Local copies of the data, passed in from Form1.
        private List<Transaction> allTransactions;
        private List<Category> allCategories;
        private List<BudgetRule> allBudgetRules;
        // This list will hold the transactions specifically for the bottom grid, after I've filtered them by date.
        private List<Transaction> filteredTransactions;

        #region Data Structures - My own little helpers for organizing data

        /// <summary>
        /// A small helper class just for my pie chart logic.
        /// It bundles together the total amount and the color for one slice of the pie.
        /// </summary>
        private class CategorySpendingDetail
        {
            public decimal Total { get; set; }
            public string ColorHex { get; set; }
        }

        /// <summary>
        /// This is a big one. I made this 'struct' to hold ALL the calculated numbers for the dashboard.
        /// Instead of having a dozen separate variables, I can just create one of these,
        /// fill it up in my calculation method, and then pass it to my UI update methods. Keeps things tidy.
        /// </summary>
        private struct DashboardMetrics
        {
            public decimal CashBalance; // Overall balance up to the end of the selected period.
            public decimal TotalSpentInPeriod; // Total of 'Needs' + 'Wants' in the period.
            public decimal SavingsInPeriod; // Money put into savings in the period.
            public decimal TotalSavingsPot; // The total amount in the savings pot overall.
            public decimal IncomeInPeriod; // Total income for the selected month/year.
            public Dictionary<string, CategorySpendingDetail> SpendingByCategory; // Data for the pie chart.
            public string ActiveRuleName; // The name of the budget rule being applied.
            public decimal NeedsTarget, WantsTarget, SavingsTarget; // The target spending based on the rule and income.
            public decimal NeedsActual, WantsActual; // How much I actually spent on Needs and Wants.
            public decimal NeedsRatio, WantsRatio, SavingsRatio; // The ratios from the active rule (e.g., 0.50, 0.30, 0.20).
        }
        #endregion

        public DashboardControl()
        {
            InitializeComponent();
            SetupTransactionGridView(); // Set up my transaction grid columns when the dashboard is created.
            // This 'CellPainting' event is for my fancy custom-drawn "Amount" column. I'll handle it below.
            dgvTransactions.CellPainting += DgvTransactions_CellPainting;
        }

        #region Main Methods
        /// <summary>
        /// This is the entry point for this control. Form1 calls this and hands over all the data.
        /// </summary>
        public void LoadData(List<Transaction> transactions, List<Category> categories, List<BudgetRule> budgetRules)
        {
            // Store the data passed in from Form1 into my local lists.
            this.allTransactions = transactions;
            this.allCategories = categories;
            this.allBudgetRules = budgetRules;
            this.filteredTransactions = new List<Transaction>(allTransactions); // Initially, the filtered list is just all transactions.

            InitializeFilters(); // Set up the dropdowns for year/month.
            UpdateDashboardView(); // Calculate and display all the main dashboard stats.
            RefreshTransactionGrid(); // Populate the transaction list at the bottom.
        }

        /// <summary>
        /// This method orchestrates updating all the visual parts of the dashboard based on the current filters.
        /// </summary>
        private void UpdateDashboardView()
        {
            // Safety check: don't try to do anything if the data isn't loaded yet.
            if (allTransactions == null || cmbYear.SelectedItem == null) return;

            // Get the current filter settings from the UI.
            bool isMonthly = radMonthly.Checked;
            int year = (int)cmbYear.SelectedItem;
            int month = cmbMonth.SelectedIndex + 1; // +1 because SelectedIndex is 0-based.

            // Here's where I call my big calculation method to do all the math.
            DashboardMetrics metrics = CalculateDashboardMetrics(isMonthly, year, month);

            // Now, take the results from my 'metrics' object and put them on the screen.
            txtCashBalance.Text = metrics.CashBalance.ToString("C"); // "C" format is for currency ($1,234.56).
            txtTotalSpent.Text = metrics.TotalSpentInPeriod.ToString("C");
            txtSavingsMade.Text = metrics.SavingsInPeriod.ToString("C");
            txtTotalSavings.Text = metrics.TotalSavingsPot.ToString("C");

            PopulateSpendingPieChart(metrics.SpendingByCategory);
            UpdateAllocationProgressBars(metrics);
        }
        #endregion

        #region Calculation Logic
        /// <summary>
        /// This is the brain of the dashboard. It takes the filter settings and crunches all the numbers.
        /// </summary>
        /// <returns>A DashboardMetrics object containing all the calculated values.</returns>
        private DashboardMetrics CalculateDashboardMetrics(bool isMonthly, int year, int month)
        {
            // Create a new metrics object to hold my results.
            var result = new DashboardMetrics { SpendingByCategory = new Dictionary<string, CategorySpendingDetail>() };

            // Figure out the start and end dates for the period I'm looking at.
            DateTime periodStartDate = new DateTime(year, isMonthly ? month : 1, 1);
            DateTime periodEndDate = isMonthly ? new DateTime(year, month, DateTime.DaysInMonth(year, month)) : new DateTime(year, 12, 31);

            // Find the budget rule that's active for this period. This LINQ query is a bit complex:
            // It finds rules where the start date is on or before my period starts, AND the end date is null OR on or after my period starts.
            // Then it orders them by start date to get the most recent one.
            var activeRule = allBudgetRules?.Where(r => r.start_date.Date <= periodStartDate.Date && (r.end_date == null || r.end_date.Value.Date >= periodStartDate.Date)).OrderByDescending(r => r.start_date).FirstOrDefault();

            // If I can't find a rule, I'll just use a default 50/30/20 rule so the app doesn't crash.
            if (activeRule == null) { activeRule = new BudgetRule { name = "Default (50/30/20)", needs_ratio = 50m, wants_ratio = 30m, savings_ratio = 20m }; }

            // Store the rule info in my result object. Remember to convert ratios from 50 to 0.50.
            result.ActiveRuleName = activeRule.name;
            result.NeedsRatio = activeRule.needs_ratio / 100;
            result.WantsRatio = activeRule.wants_ratio / 100;
            result.SavingsRatio = activeRule.savings_ratio / 100;

            // These variables will track the overall balance and savings pot by iterating through ALL transactions from the beginning of time.
            decimal runningBalance = 0, runningSavingsPot = 0;
            // It's super important to sort transactions by date before calculating a running balance.
            var sortedTransactions = allTransactions.OrderBy(t => t.transaction_date).ThenBy(t => t.id);

            // FIRST PASS: Calculate the final Cash Balance and Savings Pot at the END of the selected period.
            foreach (var t in sortedTransactions)
            {
                var category = allCategories.FirstOrDefault(c => c.id == t.category_id);
                /*
                    // This is the beginner-friendly version of the line below:
                    string categoryType;

                    // First, check if the 'category' object itself exists.
                    // This is what the '?' in 'category?.type' does.
                    if (category != null)
                    {
                        // If the category exists, get its 'type' property.
                        // The 'type' could also be null, so we still need a default.
                        categoryType = category.type;
                    }
                    else
                    {
                        // If the category object was null, we can't get a type from it.
                        categoryType = null;
                    }

                    // Now, we do a final check. This is what the '?? ""' does.
                    if (categoryType == null)
                    {
                        // If, after all that, we still have a null value,
                        // use a safe empty string instead.
                        categoryType = "";
                    }
                */
                string categoryType = category?.type ?? "";

                // Only include transactions up to the end of my selected period.
                if (t.transaction_date <= periodEndDate)
                {
                    runningBalance += t.amount; // Add income, subtract expense.
                    // Savings is special. If I 'spend' money into a savings category, it leaves my cash but enters my savings pot.
                    if (categoryType == "Savings")
                    {
                        runningSavingsPot -= t.amount; // amount is negative for expense, so this adds to the pot.
                    }
                }
            }

            // These variables will track the numbers just for the selected period (e.g., just for July).
            decimal periodIncome = 0, periodNeeds = 0, periodWants = 0, periodSavings = 0;
            var periodTransactions = allTransactions.Where(t => t.transaction_date >= periodStartDate && t.transaction_date <= periodEndDate);

            // SECOND PASS: Calculate the income, spending, etc., that happened ONLY WITHIN the selected period.
            foreach (var t in periodTransactions)
            {
                var category = allCategories.FirstOrDefault(c => c.id == t.category_id);
                /*
                    // This is the beginner-friendly version of the two lines below:

                    // --- Getting the Category Type ---
                    string categoryType;
                    if (category != null && category.type != null)
                    {
                        categoryType = category.type;
                    }
                    else
                    {
                        // Default to an empty string if the category or its type is null.
                        categoryType = "";
                    }

                    // --- Getting the Category Name ---
                    string categoryName;
                    if (category != null && category.name != null)
                    {
                        categoryName = category.name;
                    }
                    else
                    {
                        // Default to "Uncategorized" if the category or its name is null.
                        categoryName = "Uncategorized";
                    }
                */
                string categoryType = category?.type ?? "";
                string categoryName = category?.name ?? "Uncategorized";

                // Income is anything with type "Income", unless it's a transfer from savings.
                if (t.type == "Income" && categoryType != "Savings") { periodIncome += t.amount; }
                else if (t.type == "Expense")
                {
                    decimal expenseAmount = -t.amount; // Make the expense amount positive for easier math.
                    if (categoryType == "Savings") { periodSavings += expenseAmount; }
                    else
                    {
                        // Tally up the spending for Needs and Wants.
                        if (categoryType == "Needs") { periodNeeds += expenseAmount; }
                        else if (categoryType == "Wants") { periodWants += expenseAmount; }

                        // Also, add this spending to my dictionary for the pie chart.
                        if (result.SpendingByCategory.ContainsKey(categoryName))
                        {
                            result.SpendingByCategory[categoryName].Total += expenseAmount;
                        }
                        else
                        {
                            /*
                                // This is the beginner-friendly version of the line below:
                                string color;

                                // First, check if the category object itself exists.
                                if (category != null && category.color != null)
                                {
                                    // If the category and its color property both exist, use that color.
                                    color = category.color;
                                }
                                else
                                {
                                    // Otherwise (if the category is null OR its color is null), use the default gray.
                                    color = "#CCCCCC";
                                }
                            */
                            string color = category?.color ?? "#CCCCCC"; // Default to gray if no color.
                            result.SpendingByCategory.Add(categoryName, new CategorySpendingDetail { Total = expenseAmount, ColorHex = color });
                        }
                    }
                }
            }

            // Now, fill out the rest of the result object with the numbers I just calculated.
            result.CashBalance = runningBalance;
            result.TotalSavingsPot = runningSavingsPot;
            result.IncomeInPeriod = periodIncome;
            result.SavingsInPeriod = periodSavings;
            result.NeedsActual = periodNeeds;
            result.WantsActual = periodWants;
            result.TotalSpentInPeriod = periodNeeds + periodWants;
            // Calculate the budget targets based on the income for the period.
            result.NeedsTarget = periodIncome * result.NeedsRatio;
            result.WantsTarget = periodIncome * result.WantsRatio;
            result.SavingsTarget = periodIncome * result.SavingsRatio;

            return result; // Return the fully populated metrics object.
        }
        #endregion

        #region UI Update Methods
        /// <summary>
        /// Updates the text and values for the three budget allocation progress bars.
        /// </summary>
        private void UpdateAllocationProgressBars(DashboardMetrics metrics)
        {
            lblPeriodIncome.Text = $"Income in Period: {metrics.IncomeInPeriod:C2}";
            lblActiveRule.Text = $"Applying Budget Rule: {metrics.ActiveRuleName}";

            lblNeedsTitle.Text = $"Needs ({metrics.NeedsRatio:P0} Target)"; // "P0" format is for percentage with no decimals (50%).
            lblNeedsSpending.Text = $"{metrics.NeedsActual:C2} / {metrics.NeedsTarget:C2}";
            SetProgressBar(progressNeeds, metrics.NeedsActual, metrics.NeedsTarget);

            lblWantsTitle.Text = $"Wants ({metrics.WantsRatio:P0} Target)";
            lblWantsSpending.Text = $"{metrics.WantsActual:C2} / {metrics.WantsTarget:C2}";
            SetProgressBar(progressWants, metrics.WantsActual, metrics.WantsTarget);

            lblSavingsTitle.Text = $"Savings ({metrics.SavingsRatio:P0} Target)";
            lblSavingsSpending.Text = $"{metrics.SavingsInPeriod:C2} / {metrics.SavingsTarget:C2}";
            SetProgressBar(progressSavings, metrics.SavingsInPeriod, metrics.SavingsTarget);
        }

        /// <summary>
        /// A helper method to safely set the value of a progress bar.
        /// </summary>
        private void SetProgressBar(ProgressBar pb, decimal actual, decimal target)
        {
            if (target <= 0) { pb.Value = 0; return; } // Avoid division by zero.
            // Calculate the percentage, but make sure it's between 0 and 100.
            int percentage = (int)Math.Max(0, Math.Min((actual / target) * 100, 100));
            pb.Value = percentage;
        }

        /// <summary>
        /// Clears and redraws the spending pie chart with new data.
        /// </summary>
        private void PopulateSpendingPieChart(Dictionary<string, CategorySpendingDetail> spendingData)
        {
            chartSpending.Series.Clear();
            chartSpending.Legends.Clear();
            var series = new Series("Spending") { ChartType = SeriesChartType.Pie, IsValueShownAsLabel = false };

            // Handle the case where there's no spending data to show.
            if (spendingData == null || spendingData.Count == 0 || spendingData.All(kv => kv.Value.Total <= 0))
            {
                series.Points.Add(new DataPoint(0, 1) { ToolTip = "No Spending Data", Color = Color.LightGray });
            }
            else
            {
                decimal totalSpending = spendingData.Sum(kv => kv.Value.Total);
                if (totalSpending > 0)
                {
                    // Order the categories from highest spending to lowest for the legend.
                    foreach (var entry in spendingData.OrderByDescending(kv => kv.Value.Total))
                    {
                        Color categoryColor;
                        try { categoryColor = ColorTranslator.FromHtml(entry.Value.ColorHex); } // Convert the hex string to a Color object.
                        catch { categoryColor = Color.LightGray; } // If the color is invalid, use gray.

                        var dataPoint = new DataPoint(0, (double)entry.Value.Total)
                        {
                            // Tooltip shows on hover, LegendText shows in the legend.
                            ToolTip = $"{entry.Key}: {entry.Value.Total:C2} ({(entry.Value.Total / totalSpending):P1})",
                            LegendText = entry.Key,
                            Color = categoryColor
                        };
                        series.Points.Add(dataPoint);
                    }
                }
            }
            chartSpending.Series.Add(series);
        }
        #endregion

        #region Filter and Grid Methods
        /// <summary>
        /// Sets up the initial values for the filter controls.
        /// </summary>
        private void InitializeFilters()
        {
            // I temporarily remove the event handlers so that changing the values here doesn't trigger a refresh.
            radMonthly.CheckedChanged -= Filter_Changed;
            radYearly.CheckedChanged -= Filter_Changed;
            cmbYear.SelectedIndexChanged -= Filter_Changed;
            cmbMonth.SelectedIndexChanged -= Filter_Changed;

            // Get all the unique years from my transactions to populate the year dropdown.
            var years = allTransactions.Select(t => t.transaction_date.Year).Distinct().OrderByDescending(y => y).ToList();
            if (years.Any())
            {
                cmbYear.DataSource = years;
                if (years.Contains(DateTime.Now.Year)) { cmbYear.SelectedItem = DateTime.Now.Year; }
            }
            else
            {
                // If there are no transactions, just add the current year.
                cmbYear.Items.Add(DateTime.Now.Year);
                cmbYear.SelectedItem = DateTime.Now.Year;
            }
            // Populate the month dropdown and set it to the current month.
            cmbMonth.DataSource = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            cmbMonth.SelectedIndex = DateTime.Now.Month - 1;

            radMonthly.Checked = true; // Default to monthly view.

            // Now I can add the event handlers back.
            radMonthly.CheckedChanged += Filter_Changed;
            radYearly.CheckedChanged += Filter_Changed;
            cmbYear.SelectedIndexChanged += Filter_Changed;
            cmbMonth.SelectedIndexChanged += Filter_Changed;

            // Set up the date pickers for the transaction grid filter.
            dtpFrom.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpTo.Value = DateTime.Now;

            // Hook up the filter buttons to the refresh method using a lambda expression.
            btnApplyDateFilter.Click += (s, e) => RefreshTransactionGrid();
            btnResetDateFilter.Click += (s, e) => { dtpFrom.Value = new DateTime(DateTime.Now.Year, 1, 1); dtpTo.Value = DateTime.Now; RefreshTransactionGrid(); };
        }

        /// <summary>
        /// This runs whenever I change the top-level filters (Monthly/Yearly, Year, Month).
        /// </summary>
        private void Filter_Changed(object sender, EventArgs e)
        {
            cmbMonth.Enabled = radMonthly.Checked; // The month dropdown should only be enabled for monthly view.
            UpdateDashboardView(); // Recalculate and update the top part of the dashboard.
        }

        /// <summary>
        /// Defines the columns and appearance of the transaction grid. Runs only once.
        /// </summary>
        private void SetupTransactionGridView()
        {
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.Columns.Clear();
            dgvTransactions.AutoGenerateColumns = false; // I want to define my columns manually.

            // This is an important setting to prevent the grid from resizing my columns weirdly.
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Manually add each column with a specific width.
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Date", Name = "Date", Width = 85 });
            // The Description column is special. I set it to 'Fill' so it takes up any leftover space.
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Description", Name = "Description", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Category", Name = "Category", Width = 150 });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Account : Amount", Name = "Amount", Width = 130 });
            // Add my Edit and Delete buttons.
            dgvTransactions.Columns.Add(new DataGridViewButtonColumn { HeaderText = "Edit", Text = "Edit", UseColumnTextForButtonValue = true, Name = "Edit", Width = 45 });
            dgvTransactions.Columns.Add(new DataGridViewButtonColumn { HeaderText = "Delete", Text = "Delete", UseColumnTextForButtonValue = true, Name = "Delete", Width = 55 });

            // Tell the grid which method to call when a button is clicked.
            dgvTransactions.CellContentClick += DgvTransactions_CellContentClick;
        }

        /// <summary>
        /// Clears and repopulates the transaction grid based on the date picker filters.
        /// </summary>
        private void RefreshTransactionGrid()
        {
            if (allTransactions is null) return;

            // Use LINQ to filter the master transaction list by the date pickers.
            filteredTransactions = allTransactions.Where(t => t.transaction_date.Date >= dtpFrom.Value.Date && t.transaction_date.Date <= dtpTo.Value.Date).ToList();
            var sortedData = filteredTransactions.OrderByDescending(t => t.transaction_date).ToList();

            dgvTransactions.Rows.Clear(); // Clear out the old rows.

            foreach (var trans in sortedData)
            {
                // Find the category name for this transaction.
                var category = allCategories.FirstOrDefault(c => c.id == trans.category_id);
                /*
                    // This is the beginner-friendly version of the line below:
                    string categoryName;

                    // First, check if the category object itself exists and has a name.
                    if (category != null && category.name != null)
                    {
                        // If it does, use its name.
                        categoryName = category.name;
                    }
                    else
                    {
                        // Otherwise (if the category is null OR its name is null),
                        // use the default "Uncategorized" string.
                        categoryName = "Uncategorized";
                    }
                */
                string categoryName = category?.name ?? "Uncategorized";
                string dateDisplay = trans.transaction_date.ToShortDateString();

                // Add a new row to the grid. I leave the Amount column blank because I'm going to custom-draw it.
                int rowIndex = dgvTransactions.Rows.Add(dateDisplay, trans.description, categoryName, "");
                // The .Tag property is a great place to store the full Transaction object for later use (like in the edit/delete click event).
                dgvTransactions.Rows[rowIndex].Tag = trans;

                // If it's a savings transaction, I make the row taller to fit my two lines of text.
                if (category != null && category.type == "Savings")
                {
                    dgvTransactions.Rows[rowIndex].Height = 36;
                }
            }
        }

        /// <summary>
        /// This is an advanced method. It gives me complete control over how a cell is drawn.
        /// I'm using it to create a special two-line, two-color display for savings transactions in the 'Amount' column.
        /// </summary>
        private void DgvTransactions_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Only run this code for the 'Amount' column.
            if (e.RowIndex < 0 || e.ColumnIndex != dgvTransactions.Columns["Amount"].Index)
            {
                return;
            }

            e.Handled = true; // I'm handling the painting, so the grid shouldn't do its default painting.
            e.PaintBackground(e.ClipBounds, true); // Paint the cell background (e.g., selection color).

            if (dgvTransactions.Rows[e.RowIndex].Tag is Transaction transaction)
            {
                /*
                    // This is the beginner-friendly version of the line below, broken into steps:

                    // Step 1: Find the matching category in the list.
                    // FirstOrDefault will search the 'allCategories' list. If it finds a category
                    // whose 'id' matches the transaction's 'category_id', it will return that
                    // category object. If no match is found, it will return null.
                    Category foundCategory = allCategories.FirstOrDefault(c => c.id == transaction.category_id);

                    // Step 2: Safely get the type from the found category.
                    string categoryType;
                    if (foundCategory != null)
                    {
                        // If we found a category, get its 'type' property.
                        // This is what the '?.' in '...?.type' does.
                        categoryType = foundCategory.type;
                    }
                    else
                    {
                        // If we didn't find a category, the type is null.
                        categoryType = null;
                    }
    
                    // Step 3: Provide a final default value.
                    // This is what the '?? ""' does. If, after all the steps above,
                    // our 'categoryType' is still null, we make sure it becomes a safe,
                    // empty string instead of null to prevent errors later.
                    if (categoryType == null)
                    {
                        categoryType = "";
                    }
                */
                string categoryType = allCategories.FirstOrDefault(c => c.id == transaction.category_id)?.type ?? "";
                string formattedAmount = $"{Math.Abs(transaction.amount):C2}";

                // SPECIAL CASE: Savings Transactions
                if (categoryType == "Savings")
                {
                    string line1, line2;
                    Brush brush1, brush2;

                    // If it's an 'expense' into savings (money from cash TO savings)
                    if (transaction.type == "Expense")
                    {
                        line1 = $"Cash: -{formattedAmount}";
                        line2 = $"Savings: +{formattedAmount}";
                        brush1 = Brushes.Red;
                        brush2 = Brushes.Green;
                    }
                    else // It's an 'income' from savings (money FROM savings to cash)
                    {
                        line1 = $"Cash: +{formattedAmount}";
                        line2 = $"Savings: -{formattedAmount}";
                        brush1 = Brushes.Green;
                        brush2 = Brushes.Red;
                    }
                    // A bit of math to center the two lines of text vertically in the cell.
                    Font cellFont = e.CellStyle.Font;
                    SizeF line1Size = e.Graphics.MeasureString(line1, cellFont);
                    float totalHeight = line1Size.Height * 2;
                    float startingY = e.CellBounds.Top + (e.CellBounds.Height - totalHeight) / 2;
                    // Draw the two strings manually.
                    e.Graphics.DrawString(line1, cellFont, brush1, e.CellBounds.Left + 2, startingY);
                    e.Graphics.DrawString(line2, cellFont, brush2, e.CellBounds.Left + 2, startingY + line1Size.Height);
                }
                else // NORMAL CASE: Regular Income/Expense
                {
                    string text = $"Cash: {(transaction.type == "Income" ? "+" : "-")}{formattedAmount}";
                    Brush brush = (transaction.type == "Income") ? Brushes.Green : Brushes.Red;
                    // Use a StringFormat object to center the text vertically.
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center
                    };
                    e.Graphics.DrawString(text, e.CellStyle.Font, brush, e.CellBounds, sf);
                }
            }
        }

        /// <summary>
        /// This event fires when I click on content inside a cell, specifically my Edit and Delete buttons.
        /// </summary>
        private async void DgvTransactions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Ignore clicks on the header.

            // Get the full Transaction object I stored in the row's Tag property.
            if (dgvTransactions.Rows[e.RowIndex].Tag is Transaction transaction)
            {
                // Check which button was clicked by looking at the column's name.
                if (dgvTransactions.Columns[e.ColumnIndex].Name == "Delete")
                {
                    // Always ask for confirmation before deleting!
                    if (MessageBox.Show($"Are you sure you want to delete this transaction?\n\n'{transaction.description}'", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        try
                        {
                            var service = new WebService1SoapClient();
                            await service.DeleteTransactionAsync(transaction.id); // Call the server to delete it.
                            DataChanged?.Invoke(this, EventArgs.Empty); // Fire my event to tell Form1 to reload everything.
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to delete transaction. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (dgvTransactions.Columns[e.ColumnIndex].Name == "Edit")
                {
                    // Open the same TransactionDialog I use for adding, but this time pass in the existing transaction to pre-fill the fields.
                    using (var dialog = new TransactionDialog(allCategories, transaction))
                    {
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                var updatedTransaction = dialog.ResultTransaction;
                                var service = new WebService1SoapClient();
                                await service.UpdateTransactionAsync(updatedTransaction.id, updatedTransaction.description, updatedTransaction.amount, updatedTransaction.type, updatedTransaction.category_id, updatedTransaction.transaction_date);
                                DataChanged?.Invoke(this, EventArgs.Empty); // Tell Form1 to reload.
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Failed to update transaction. Error: {ex.Message}", "API Error");
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}

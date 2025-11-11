// Bringing in my toolkits. System.Drawing is needed for the Color object.
using System;
using System.Collections.Generic;   // For lists and dictionaries to hold my transactions, categories, and budget rules.
using System.Drawing;   // For the Color object used in categories.
using System.Linq;  // For LINQ operations like sorting and filtering.
using System.Windows.Forms;
using client_app.ServiceReference1;

namespace client_app
{
    /// <summary>
    /// This is my 'page' for managing the transaction categories.
    /// It's a UserControl that I plug into the main form.
    /// Here, I can see all my categories, add new ones, edit them, and delete them.
    /// </summary>
    public partial class CategoriesControl : UserControl
    {
        // My "call home" event. When I make a change, I fire this to tell Form1 to reload all the data.
        public event EventHandler DataChanged;

        #region Private Fields
        // This is my local copy of all the categories, passed in from Form1.
        private List<Category> allCategories;
        // This variable will hold the color I pick from the color dialog for a new category.
        private Color selectedColor = Color.LightGray;
        #endregion

        public CategoriesControl()
        {
            InitializeComponent();
            SetupDataGridView(); // Configure the grid's columns and appearance.
            btnColor.BackColor = selectedColor; // Set the initial color of the color-picker button.

            // Hook up my button clicks to their corresponding methods.
            btnAddCategory.Click += BtnAddCategory_Click;
            btnColor.Click += BtnColor_Click;
            dgvCategories.CellContentClick += DgvCategories_CellContentClick; // For the Edit/Delete buttons in the grid.
        }

        /// <summary>
        /// This is the entry point for this control. Form1 calls this and gives me the list of categories.
        /// </summary>
        public void LoadCategories(List<Category> categories)
        {
            // Store the list locally. The '??' makes sure that if Form1 sends a null list, I just use a new empty one to avoid errors.
            /*
                // This is the beginner-friendly version of the line below:
                if (categories != null)
                {
                    // If the list from the main form is valid, use it.
                    this.allCategories = categories;
                }
                else
                {
                    // If the list is null, create a new, empty list to be safe.
                    this.allCategories = new List<Category>();
                }
            */
            this.allCategories = categories ?? new List<Category>();
            RefreshCategoryGrid(); // Now, fill the grid with these categories.
        }

        /// <summary>
        /// This method clears and re-populates the grid with the current list of categories.
        /// </summary>
        private void RefreshCategoryGrid()
        {
            if (allCategories == null) return; // Safety check.

            // Use LINQ to sort the categories alphabetically by name before displaying them.
            var sortedData = allCategories
                .OrderBy(c => c.name)
                .ToList();

            dgvCategories.Rows.Clear(); // Clear out any old data.
            foreach (var cat in sortedData)
            {
                // Add a new row for each category.
                int rowIndex = dgvCategories.Rows.Add(cat.name, cat.description, cat.type);

                // I need a try-catch block here because the color string from the database could be invalid.
                try
                {
                    // Set the background color of the "Color" cell.
                    /*
                        // This is the beginner-friendly version of the line below:
                        string colorCode;

                        if (cat.color != null)
                        {
                            // If a color code exists in the database, use it.
                            colorCode = cat.color;
                        }
                        else
                        {
                            // Otherwise, use a default gray color.
                            colorCode = "#CCCCCC";
                        }

                        // Now, convert the color code string into a real Color object
                        // and set the cell's background.
                        Color cellColor = ColorTranslator.FromHtml(colorCode);
                        dgvCategories.Rows[rowIndex].Cells["Color"].Style.BackColor = cellColor;
                    */
                    dgvCategories.Rows[rowIndex].Cells["Color"].Style.BackColor = ColorTranslator.FromHtml(cat.color ?? "#CCCCCC");
                }
                catch
                {
                    // If the color is bad, just default to gray.
                    dgvCategories.Rows[rowIndex].Cells["Color"].Style.BackColor = Color.LightGray;
                }

                // And again, the .Tag property is my best friend for storing the full object.
                dgvCategories.Rows[rowIndex].Tag = cat;
            }
        }

        #region Setup and Event Handlers
        /// <summary>
        /// Configures the columns and appearance of the DataGridView. Runs only once.
        /// </summary>
        private void SetupDataGridView()
        {
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.RowHeadersVisible = false;
            dgvCategories.Columns.Clear();
            dgvCategories.AutoGenerateColumns = false; // I'll define my own columns.
            dgvCategories.ScrollBars = ScrollBars.Vertical;

            // Define my columns with specific widths to make the layout look good.
            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Name", Name = "Name", Width = 200 });
            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Description", Name = "Description", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Type", Name = "Type", Width = 100 });
            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Color", Name = "Color", ReadOnly = true, Width = 50 });
            dgvCategories.Columns.Add(new DataGridViewButtonColumn { HeaderText = "Edit", Text = "Edit", UseColumnTextForButtonValue = true, Name = "Edit", Width = 60 });
            dgvCategories.Columns.Add(new DataGridViewButtonColumn { HeaderText = "Delete", Text = "Delete", UseColumnTextForButtonValue = true, Name = "Delete", Width = 60 });
        }

        /// <summary>
        /// Opens the standard Windows color picker dialog when the color button is clicked.
        /// </summary>
        private void BtnColor_Click(object sender, EventArgs e)
        {
            // The 'using' block makes sure the dialog is disposed of correctly.
            using (ColorDialog colorDialog = new ColorDialog())
            {
                // If I pick a color and click OK...
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    //...store the chosen color and update the button's background to give visual feedback.
                    selectedColor = colorDialog.Color;
                    btnColor.BackColor = selectedColor;
                }
            }
        }

        /// <summary>
        /// Runs when I click the "Add Category" button.
        /// </summary>
        private async void BtnAddCategory_Click(object sender, EventArgs e)
        {
            // Simple validation to make sure the required fields are filled.
            if (string.IsNullOrWhiteSpace(txtName.Text) || cmbType.SelectedItem == null)
            {
                MessageBox.Show("Please provide at least a name and a type for the category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop if validation fails.
            }

            try
            {
                var service = new WebService1SoapClient();
                // My server expects a hex string (e.g., "#FF5733"), so I need to convert the Color object.
                // The ':X2' format ensures the R, G, and B values are always two-digit hex numbers.
                string hexColor = $"#{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";

                // Call the server to add the new category.
                await service.AddCategoryAsync(txtName.Text, cmbType.SelectedItem.ToString(), txtDescription.Text, hexColor);

                MessageBox.Show("Category added successfully!", "Success");

                // Clear out the input form so I can add another one.
                txtName.Clear();
                txtDescription.Clear();
                cmbType.SelectedIndex = -1;
                selectedColor = Color.LightGray;
                btnColor.BackColor = selectedColor;

                // Tell Form1 that data has changed and a reload is needed.
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add category. Error: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Runs when I click a button (Edit or Delete) inside the grid.
        /// </summary>
        private async void DgvCategories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignore clicks on the header.

            // Get the full Category object from the row's Tag.
            var category = dgvCategories.Rows[e.RowIndex].Tag as Category;
            if (category == null) return;

            var service = new WebService1SoapClient();

            // Check which column's button was clicked.
            if (dgvCategories.Columns[e.ColumnIndex].Name == "Delete")
            {
                var confirmResult = MessageBox.Show($"Are you sure you want to delete '{category.name}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        await service.DeleteCategoryAsync(category.id);
                        DataChanged?.Invoke(this, EventArgs.Empty); // Reload on success.
                    }
                    catch (Exception)
                    {
                        // REMINDER: This is good error handling for a specific business rule.
                        // The database will throw an error if I try to delete a category that's linked to a transaction (a foreign key constraint).
                        // I can catch that error and show a much friendlier message to the user.
                        MessageBox.Show("This category cannot be deleted because it is being used by one or more transactions.", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (dgvCategories.Columns[e.ColumnIndex].Name == "Edit")
            {
                // There is another form called 'EditCategoryForm'.
                // I open it and pass the category I want to edit.
                using (var editForm = new EditCategoryForm(category))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            var updatedCategory = editForm.UpdatedCategory;
                            await service.UpdateCategoryAsync(updatedCategory.id, updatedCategory.name, updatedCategory.type, updatedCategory.description, updatedCategory.color);
                            DataChanged?.Invoke(this, EventArgs.Empty); // Reload on success.
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to update category. Error: {ex.Message}", "API Error");
                        }
                    }
                }
            }
        }
        #endregion
    }
}

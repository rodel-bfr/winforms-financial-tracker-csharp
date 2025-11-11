// Just need the basics for this form: System for general stuff, Drawing for Color, and WinForms for the UI controls.
using System;
using System.Drawing;
using System.Windows.Forms;
using client_app.ServiceReference1;

namespace client_app
{
    /// <summary>
    /// This is a dedicated pop-up form just for editing an existing category.
    /// It's simpler than the other dialogs because it doesn't need to handle "add" mode.
    /// </summary>
    public partial class EditCategoryForm : Form
    {
        // This will hold the color I pick from the color dialog.
        private Color selectedColor;

        // This is the public 'output' of this form.
        // It's a property that the CategoriesControl can access after the dialog closes
        // to get the final, updated category data.
        public Category UpdatedCategory { get; private set; }

        /// <summary>
        /// The constructor for this form. It requires a Category object to be passed in.
        /// </summary>
        /// <param name="categoryToEdit">The category object that the user wants to edit.</param>
        public EditCategoryForm(Category categoryToEdit)
        {
            InitializeComponent();

            // I immediately store the category that was passed in.
            // This object will be updated directly when the user clicks save.
            this.UpdatedCategory = categoryToEdit;

            // Hook up my button click events.
            btnColor.Click += BtnColor_Click;
            btnSave.Click += BtnSave_Click;

            // Call my method to fill the form controls with the category's existing data.
            LoadCategoryData();
        }

        /// <summary>
        /// This method takes the data from the 'UpdatedCategory' object and uses it
        /// to pre-fill all the text boxes and controls on the form.
        /// </summary>
        private void LoadCategoryData()
        {
            // Set the text for the name and description.
            txtName.Text = UpdatedCategory.name;
            txtDescription.Text = UpdatedCategory.description;

            // Set the selected item for the Type combo box.
            cmbType.SelectedItem = UpdatedCategory.type;

            // Set the color for the color button.
            // It's important to use a try-catch here in case the color string from the
            // database is invalid. This prevents the app from crashing.
            try
            {
                // The '??' operator provides a default color if the one from the database is null.
                /*
                    // This is the beginner-friendly version of the line below:

                    // Step 1: Decide which color code to use.
                    string colorCodeToUse;
                    if (UpdatedCategory.color != null)
                    {
                        // If the category has a color saved, use it.
                        colorCodeToUse = UpdatedCategory.color;
                    }
                    else
                    {
                        // Otherwise, use a default gray color. This is what '?? "#CCCCCC"' does.
                        colorCodeToUse = "#CCCCCC";
                    }

                    // Step 2: Convert the chosen color code string into a Color object.
                    // This is what 'ColorTranslator.FromHtml(...)' does.
                    selectedColor = ColorTranslator.FromHtml(colorCodeToUse);
                */
                selectedColor = ColorTranslator.FromHtml(UpdatedCategory.color ?? "#CCCCCC");
            }
            catch
            {
                // If FromHtml fails, just use a safe default color.
                selectedColor = Color.LightGray;
            }
            // Update the button's background to show the loaded color.
            btnColor.BackColor = selectedColor;
        }

        /// <summary>
        /// Runs when I click the color swatch button. Opens the color picker dialog.
        /// </summary>
        private void BtnColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                // This is a nice touch: pre-select the category's current color in the dialog.
                colorDialog.Color = selectedColor;

                // If the user picks a color and clicks OK...
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    // ...store the new color and update the button's appearance.
                    selectedColor = colorDialog.Color;
                    btnColor.BackColor = selectedColor;
                }
            }
        }

        /// <summary>
        /// Runs when I click the "Save Changes" button.
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // --- REMINDER: Simple validation is essential! ---
            if (string.IsNullOrWhiteSpace(txtName.Text) || cmbType.SelectedItem == null)
            {
                MessageBox.Show("Name and Type cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop the save if validation fails.
            }

            // If validation passes, I update the public 'UpdatedCategory' object
            // with the new values from the form's controls.
            UpdatedCategory.name = txtName.Text;
            UpdatedCategory.description = txtDescription.Text;
            UpdatedCategory.type = cmbType.SelectedItem.ToString();
            // Convert the selected Color object back into a hex string for the database.
            UpdatedCategory.color = $"#{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";

            // This is the signal to the CategoriesControl that the user finished and saved the changes.
            this.DialogResult = DialogResult.OK;
            // This closes the pop-up form.
            this.Close();
        }
    }
}

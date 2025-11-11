// These are all the toolkits I need from the .NET Framework.
// Like, I need List<> for my collections, and all the Sql stuff to talk to the database.
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Services;

// My project's main container. Keeps my code organized.
namespace server_app
{
    // region is just a way to collapse code in Visual Studio to keep things tidy.
    #region Data Structures - My C# blueprints for the database tables

    /// <summary>
    /// This class is just a C# version of my 'categories' table.
    /// It's a simple object to hold all the info for a single category.
    /// </summary>
    public class Category
    {
        public int id { get; set; } // The primary key from the DB.
        public string name { get; set; } // e.g., "Groceries", "Salary"
        public string description { get; set; } // A little note about the category.
        public string color { get; set; } // For making the UI look nice, like "#FF5733".
        public string type { get; set; } // This is for my budget rule, either "Needs", "Wants", or "Savings".
        public bool is_predefined { get; set; } // Was this a default category or one I added myself?
    }

    /// <summary>
    /// This matches my 'transactions' table. Holds one transaction.
    /// </summary>
    public class Transaction
    {
        public int id { get; set; } // The primary key.
        public string description { get; set; } // e.g., "Lunch with friends".
        public decimal amount { get; set; } // The money. IMPORTANT: I store expenses as negative numbers in the DB.
        public string type { get; set; } // "Income" or "Expense".
        public int? category_id { get; set; } // The foreign key to the 'categories' table. The '?' means it can be null, for transactions without a category.
        public string category_name { get; set; } // This isn't in the DB table. I'll get it with a JOIN query to make life easier on the client-side.
        public DateTime transaction_date { get; set; } // When the transaction happened.
    }

    /// <summary>
    /// A special class just for my dashboard chart.
    /// It's not a direct table map; it's a summary of data.
    /// </summary>
    public class CategoryTotal
    {
        public string Name { get; set; } // Category name, e.g., "Food".
        public string Type { get; set; } // "Needs" or "Wants".
        public decimal Total { get; set; } // The total money spent in that category.
    }

    /// <summary>
    /// Blueprint for my 'budget_rules' table. This is for stuff like the 50/30/20 rule.
    /// </summary>
    public class BudgetRule
    {
        public int id { get; set; } // Primary key.
        public string name { get; set; } // e.g., "My 50/30/20 Plan".
        public DateTime start_date { get; set; } // When this rule starts.
        public DateTime? end_date { get; set; } // When this rule ends. The '?' means it can be null if the rule doesn't have an end date.
        public decimal needs_ratio { get; set; } // The % for needs. I'll get this as 50 but store it as 0.50 in the DB.
        public decimal wants_ratio { get; set; } // The % for wants.
        public decimal savings_ratio { get; set; } // The % for savings.
    }
    #endregion


    /// <summary>
    /// This is the main brain of my server app. All the client app's requests come here.
    /// This is an old-school ASMX Web Service.
    /// </summary>
    [WebService(Namespace = "http://services.financial-tracker.com/")] // Just a unique name for my service.
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)] // Makes sure it plays nice with other systems.
    [System.ComponentModel.ToolboxItem(false)] // I don't need this showing up in my Visual Studio toolbox.
    public class WebService1 : System.Web.Services.WebService
    {
        // This is my database connection string. It's like the address and password to my database.
        // REMINDER: Hard-coding this is bad practice for a real app. It should go in the Web.config file!
        private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Rodel\Desktop\win-app-financial-tracker\server_app\server_app\App_Data\Database1.mdf;Integrated Security=True";

        #region Category Methods - Everything for Creating, Reading, Updating, Deleting Categories
        /// <summary>
        /// This is the function that gets all my categories from the DB.
        /// </summary>
        [WebMethod] // This little tag makes the function available to my client app.
        public List<Category> GetCategories()
        {
            var categories = new List<Category>(); // Create an empty list to put my results in.
            // The 'using' block is a lifesaver. It automatically closes the database connection for me, even if there's an error.
            using (var con = new SqlConnection(connectionString))
            {
                // My SQL command to grab everything from the categories table.
                string query = "SELECT * FROM categories ORDER BY type, name";
                using (var cmd = new SqlCommand(query, con))
                {
                    con.Open(); // Open the connection.
                    var reader = cmd.ExecuteReader(); // Run the query and get the results back row-by-row.
                    // Loop through each row the database gives me.
                    while (reader.Read())
                    {
                        // For each row, create a new Category object and fill it with the data.
                        categories.Add(new Category
                        {
                            id = Convert.ToInt32(reader["id"]),
                            name = reader["name"].ToString(),
                            description = reader["description"].ToString(),
                            color = reader["color"].ToString(),
                            type = reader["type"].ToString(),
                            is_predefined = Convert.ToBoolean(reader["is_predefined"])
                        });
                    }
                }
            }
            return categories; // Send the list of categories back to the client.
        }

        /// <summary>
        /// This adds a new category to the database.
        /// </summary>
        [WebMethod]
        public int AddCategory(string name, string type, string description, string color)
        {
            using (var con = new SqlConnection(connectionString))
            {
                // This SQL is a bit clever. 'OUTPUT INSERTED.ID' makes the DB tell me the ID of the new row I just added.
                // I've hard-coded 'is_predefined' to 0 because any category added by a user is not predefined.
                string query = "INSERT INTO categories(name, type, description, color, is_predefined) OUTPUT INSERTED.ID VALUES (@name, @type, @description, @color, 0)";
                using (var cmd = new SqlCommand(query, con))
                {
                    // REMINDER: ALWAYS use parameters like this (@name, @type). It prevents a major security risk called SQL Injection.
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@color", color);
                    con.Open();
                    // ExecuteScalar is perfect here because I'm only expecting one single value back (the new ID).
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// This method finds a category by its ID and updates its info.
        /// </summary>
        [WebMethod]
        public void UpdateCategory(int id, string name, string type, string description, string color)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string query = "UPDATE categories SET name = @name, type = @type, description = @description, color = @color WHERE id = @id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@color", color);
                    con.Open();
                    // ExecuteNonQuery is for when I'm not expecting any data back (like for an UPDATE or DELETE).
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Bye bye category. Deletes one from the database using its ID.
        /// </summary>
        [WebMethod]
        public void DeleteCategory(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM categories WHERE id = @id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Transaction Methods - All the logic for handling money in and money out
        /// <summary>
        /// Gets all the transactions.
        /// </summary>
        [WebMethod]
        public List<Transaction> GetTransactions()
        {
            var transactions = new List<Transaction>();
            using (var con = new SqlConnection(connectionString))
            {
                // This 'LEFT JOIN' is key. It lets me pull the category name from the 'categories' table and put it into my Transaction object.
                // I use 'LEFT' join so if a transaction has no category, it still shows up in my list.
                string query = "SELECT t.*, c.name as category_name FROM transactions t LEFT JOIN categories c ON t.category_id = c.id ORDER BY t.transaction_date DESC, t.id DESC";
                using (var cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        transactions.Add(new Transaction
                        {
                            id = Convert.ToInt32(reader["id"]),
                            description = reader["description"].ToString(),
                            amount = Convert.ToDecimal(reader["amount"]),
                            type = reader["type"].ToString(),
                            // This part is a bit tricky. The database has NULLs, but C# needs to know how to handle them.
                            // This line says: if the 'category_id' from the DB is null, make my C# property null. Otherwise, convert it to a number.
                            category_id = reader["category_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["category_id"]),
                            category_name = reader["category_name"].ToString(), // This is the field I got from the JOIN.
                            transaction_date = Convert.ToDateTime(reader["transaction_date"])
                        });
                    }
                }
            }
            return transactions;
        }

        /// <summary>
        /// Adds a new transaction.
        /// </summary>
        [WebMethod]
        public int AddTransaction(string description, decimal amount, string type, int? category_id, DateTime transaction_date)
        {
            using (var con = new SqlConnection(connectionString))
            {
                // MY RULE: Expenses are negative, income is positive. This line enforces that.
                // It takes the absolute value first to be safe, then makes it negative if it's an expense.
                decimal finalAmount = type.Equals("Expense", StringComparison.OrdinalIgnoreCase) ? -Math.Abs(amount) : Math.Abs(amount);
                string query = "INSERT INTO transactions(description, amount, type, category_id, transaction_date) OUTPUT INSERTED.ID VALUES (@description, @amount, @type, @category_id, @transaction_date)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@amount", finalAmount);
                    cmd.Parameters.AddWithValue("@type", type);
                    // Here's how to handle a nullable parameter for the database. If my category_id is null, I send DBNull.Value.
                    cmd.Parameters.AddWithValue("@category_id", (object)category_id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@transaction_date", transaction_date);
                    con.Open();
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Updates an existing transaction.
        /// </summary>
        [WebMethod]
        public void UpdateTransaction(int id, string description, decimal amount, string type, int? category_id, DateTime transaction_date)
        {
            using (var con = new SqlConnection(connectionString))
            {
                // Same rule as adding: make sure expenses are stored as negative numbers.
                decimal finalAmount = type.Equals("Expense", StringComparison.OrdinalIgnoreCase) ? -Math.Abs(amount) : Math.Abs(amount);
                string query = "UPDATE transactions SET description=@description, amount=@amount, type=@type, category_id=@category_id, transaction_date=@transaction_date WHERE id=@id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@amount", finalAmount);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@category_id", (object)category_id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@transaction_date", transaction_date);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Deletes a transaction.
        /// </summary>
        [WebMethod]
        public void DeleteTransaction(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM transactions WHERE id = @id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Budget Rule Methods - Logic for the 50/30/20 rule stuff
        /// <summary>
        /// Gets all my saved budget rules.
        /// </summary>
        [WebMethod]
        public List<BudgetRule> GetBudgetRules()
        {
            var list = new List<BudgetRule>();
            using (var con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM budget_rules ORDER BY start_date DESC";
                using (var cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new BudgetRule
                        {
                            id = Convert.ToInt32(reader["id"]),
                            name = reader["name"].ToString(),
                            start_date = Convert.ToDateTime(reader["start_date"]),
                            end_date = reader["end_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["end_date"]),
                            // REMEMBER: DB stores this as a decimal (0.50), but my app uses percentages (50). So I multiply by 100 here when getting data.
                            needs_ratio = Convert.ToDecimal(reader["needs_ratio"]) * 100,
                            wants_ratio = Convert.ToDecimal(reader["wants_ratio"]) * 100,
                            savings_ratio = Convert.ToDecimal(reader["savings_ratio"]) * 100
                        });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Adds a new budget rule.
        /// </summary>
        [WebMethod]
        public int AddBudgetRule(string name, DateTime start_date, DateTime? end_date, decimal needs_ratio, decimal wants_ratio, decimal savings_ratio)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO budget_rules (name, start_date, end_date, needs_ratio, wants_ratio, savings_ratio) OUTPUT INSERTED.ID VALUES (@name, @start_date, @end_date, @needs, @wants, @savings)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@start_date", start_date);
                    cmd.Parameters.AddWithValue("@end_date", (object)end_date ?? DBNull.Value);
                    // Opposite of what I did in GetBudgetRules. Here I take the percentage from the client (e.g., 50) and divide by 100 to store it as a decimal (0.50).
                    cmd.Parameters.AddWithValue("@needs", needs_ratio / 100);
                    cmd.Parameters.AddWithValue("@wants", wants_ratio / 100);
                    cmd.Parameters.AddWithValue("@savings", savings_ratio / 100);
                    con.Open();
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Updates an existing budget rule.
        /// </summary>
        [WebMethod]
        public void UpdateBudgetRule(int id, string name, DateTime start_date, DateTime? end_date, decimal needs_ratio, decimal wants_ratio, decimal savings_ratio)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string query = "UPDATE budget_rules SET name=@name, start_date=@start_date, end_date=@end_date, needs_ratio=@needs, wants_ratio=@wants, savings_ratio=@savings WHERE id=@id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@start_date", start_date);
                    cmd.Parameters.AddWithValue("@end_date", (object)end_date ?? DBNull.Value);
                    // Don't forget to divide by 100 here too!
                    cmd.Parameters.AddWithValue("@needs", needs_ratio / 100);
                    cmd.Parameters.AddWithValue("@wants", wants_ratio / 100);
                    cmd.Parameters.AddWithValue("@savings", savings_ratio / 100);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Deletes a budget rule.
        /// </summary>
        [WebMethod]
        public void DeleteBudgetRule(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM budget_rules WHERE id=@id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Dashboard - Methods for creating summaries and reports
        /// <summary>
        /// This is for the dashboard pie chart. It calculates the total spending for each category within a date range.
        /// </summary>
        [WebMethod]
        public List<CategoryTotal> GetCategoryTotals(DateTime startDate, DateTime endDate)
        {
            var totals = new List<CategoryTotal>();
            using (var con = new SqlConnection(connectionString))
            {
                // This is a more complex query. Let's break it down:
                // SELECT c.name, c.type, SUM(t.amount) as total -> Get the category name, its type, and the sum of transaction amounts.
                // FROM transactions t JOIN categories c ON t.category_id = c.id -> Combine transactions and categories tables.
                // WHERE ... -> Filter by date range and only look at 'Expense' transactions.
                // GROUP BY c.name, c.type -> This is the magic. It groups all rows with the same category name and type together so SUM() can work on each group.
                var query = @"
                    SELECT c.name, c.type, SUM(t.amount) as total 
                    FROM transactions t 
                    JOIN categories c ON t.category_id = c.id
                    WHERE t.transaction_date BETWEEN @startDate AND @endDate AND t.type = 'Expense'
                    GROUP BY c.name, c.type";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            totals.Add(new CategoryTotal
                            {
                                Name = reader["name"].ToString(),
                                Type = reader["type"].ToString(),
                                // Since expenses are stored as negative, I use Math.Abs() to make the total a positive number for the chart.
                                Total = Math.Abs(Convert.ToDecimal(reader["total"]))
                            });
                        }
                    }
                }
            }
            return totals;
        }
        #endregion
    }
}
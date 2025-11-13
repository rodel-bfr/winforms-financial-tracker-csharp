# 💰 C# Financial Tracker (Windows Forms)

This is a full-stack personal finance management application built entirely in the **C# / .NET Framework**. It features a **Windows Forms (WinForms)** desktop client that consumes a **C# ASP.NET (ASMX) Web Service** for all backend logic and database operations.

This project demonstrates the same "Financial Tracker" application logic as my React/PHP project, but re-implemented in a classic Microsoft technology stack.

---

## ✨ Key Features

* **Dynamic Budgeting:** Supports custom budget rules, such as the **50/30/20 rule**, helping users balance **Needs**, **Wants**, and **Savings**.
* **Customizable Categories:** Users can define and edit their own spending categories and allocation types.
* **Visual Analytics:** Intuitive graphs (Pie and Progress Bar charts) provide a visual breakdown of spending habits and budget adherence.
* **Transaction Management:** A filterable table for managing all past and present transactions, including edit and delete functionality.

---

## 🛠️ Tech Stack

* **Client:** C# Windows Forms (.NET Framework)
* **Backend:** C# ASP.NET Web Service (ASMX)
* **Database:** SQL Server LocalDB (`.mdf` file)
* **IDE:** Visual Studio 2022
* **Data:** LINQ, ADO.NET
* **Design:** `async/await` for a non-blocking UI, custom `CellPainting` (GDI+) for data grids.

---

## 📸 Application Preview

![Dashboard](https://github.com/user-attachments/assets/71741d61-e507-497f-9b06-7409e1330af9)

![Categories](https://github.com/user-attachments/assets/429fc6f4-b62f-4049-8fc3-e86cf4640347)

![Budget Rules](https://github.com/user-attachments/assets/70255aad-bf6a-4312-bfbf-808eeb0f6191)

![Add Transaction](https://github.com/user-attachments/assets/166afe97-1e37-4757-9b89-02820d4d371d)

![Server API](https://github.com/user-attachments/assets/b6104503-3dd7-4400-8f38-f39213fa069b)

---

## 💾 Database Schema (ERD)

Here is the Entity Relationship Diagram (ERD) for the database, showing the table structures and relationships.

![Database ERD](https://github.com/user-attachments/assets/0f849600-8bea-4298-a7ec-f1ad38927b18)

---

## 🚀 Local Setup Guide

To run this project, you need **Visual Studio 2022** with the following two workloads installed:
* **.NET desktop development** (for the client)
* **ASP.NET and web development** (for the server)

You also need **SQL Server Management Studio (SSMS)** to set up the database.

### 1. Database Setup

1.  Open **SSMS** and connect to your LocalDB server. The server name is: `(LocalDB)\MSSQLLocalDB`
2.  Open the `sql_files/database.sql` file from this project.
3.  Execute the entire SQL script. This will create the `Database1` database, all tables, and insert all the demo data.

### 2. Server Setup (Run First)

1.  Open the `server_app/server_app.sln` solution file in Visual Studio.
2.  **CRITICAL:** In the Solution Explorer, open the `web.config` file.
3.  Find the `<connectionStrings>` section. Make sure the `AttachDbFilename` path points to *your* local `.mdf` file location. (You may need to update this path).
    * Example: `AttachDbFilename=C:\Users\Rodel\Desktop\win-app-financial-tracker\server_app\server_app\App_Data\Database1.mdf`
4.  Press **F5** or the "Run" button to start the server. A browser window will open showing the "WebService1" page.
5.  **Note the URL in your browser** (e.g., `https://localhost:44349/`).
6.  **Leave this server running.**

### 3. Client Setup (Run Second)

1.  Open a **second window** of Visual Studio.
2.  Open the `clinet_app/clinet_app.sln` solution file.
3.  **CRITICAL:** In the Solution Explorer, open the `App.config` file.
4.  Find the `<endpoint address="..."` line.
5.  Make sure the address **exactly matches** the URL your server is running on (from Step 2.5).
    * Example: `address="https://localhost:44349/WebService1.asmx"`
6.  Press **F5** to run the client. The app will launch and connect to your local server.
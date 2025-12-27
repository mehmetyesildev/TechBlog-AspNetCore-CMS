# 🚀 TechBlog - Modern ASP.NET Core CMS

![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-purple?style=flat&logo=dotnet) ![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-blue?style=flat&logo=nuget) ![Bootstrap](https://img.shields.io/badge/Bootstrap-5-orange?style=flat&logo=bootstrap) ![License](https://img.shields.io/badge/License-Non%20Commercial-red)

TechBlog is a dynamic content management system developed from scratch using **ASP.NET Core 8.0** architecture. Built upon the principles of **N-Tier Architecture**, the project has been enhanced with **Role-Based Authorization (RBAC)**, **AJAX-based interactions**, and a **Modern UI**.

---

## 🌟 Key Features & Technical Insights

### 🏗️ Backend & Architecture
* **Entity Framework Core (Code First):** Database entities designed as C# classes and managed via `Migrations`.
* **Repository Pattern:** Implemented to decouple business logic from data access, ensuring cleaner and testable code (`PostsRepository`).
* **Data Modeling:** Optimized One-to-Many (Author-Post, Post-Comment) and Many-to-Many (Post-Tag) relationships.
* **Seed Data:** Automated test data generation upon initial application startup.

### 🔐 Security & Authentication
* **Cookie-Based Authentication:** Secure session management.
* **Role-Based Authorization (RBAC):**
    * **Admin:** Full access to manage all content (Active/Passive), users, and roles.
    * **User:** Can only view and manage their own content; isolated from administrative functions.
* **Backend Validation:** Prevents unauthorized access via URL manipulation (IDor Protection).

### 🎨 Frontend & UI
* **ViewComponents:** Modular design for Sidebar, Popular Posts, and Tag Cloud.
* **AJAX & jQuery:** Asynchronous comment submission and listing without page reloads.
* **Modern Design:** Responsive card layouts utilizing **Bootstrap 5**.
* **State Management (UI):** Visual badges for content status (Published/Draft) and user roles.

### ⚙️ Content Management System (CMS)
* **Rich Text Editor:** Integrated **TinyMCE** for HTML-formatted blog post creation.
* **File Management:** Server-side image upload handling via `IFormFile`.
* **Dynamic Filtering:** Content filtering by Tags or Categories via URL routing.

---

## 📸 Screenshots

### 1. Role-Based Authorization (Admin vs User)
Visual proof of backend security. The Admin manages the entire system, while standard users are isolated to their own data.

| **Admin Panel (Full Access)** | **Standard User Panel (Restricted)** |
|:---:|:---:|
| <img src="https://github.com/user-attachments/assets/55edc30b-a2d2-4af7-b7d1-7f057d05fc61" height="300"> | <img src="https://github.com/user-attachments/assets/695aba38-903d-4e81-9455-979ae1de6266" height="300"> |
| *Admin manages all authors and statuses.* | *Users only see their own posts.* |

### 2. Relational Data & Details
Reflection of database relationships (Joins) on the UI. The content and interaction areas are designed separately.

| **Rich Text Content (Html.Raw)** | **Interactive Comment System (AJAX)** |
|:---:|:---:|
| <img src="https://github.com/user-attachments/assets/dc7ae519-dea3-41bb-ae75-d285a5af3832" height="450"> | <img src="https://github.com/user-attachments/assets/72763bb4-2b11-4c34-8020-9ccf496599a6" height="450"> |
| *Blog posts support HTML content.* | *Comments load asynchronously.* |

### 3. Content Creation Panel
TinyMCE editor and file upload mechanism.

<div align="center">
  <img src="https://github.com/user-attachments/assets/66e50e0c-d1d4-4d2f-9193-62042f4845fe" width="85%">
</div>

### 4. Modern Showcase (Home Page)
Responsive home page with pagination infrastructure.

<div align="center">
  <img src="https://github.com/user-attachments/assets/e9060a7e-aefc-4d18-8790-de15fc8cd66f" width="85%">
</div>

---

## 🛠️ Installation & Setup

To run this project locally, follow these steps:

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/mehmetyesildev/TechBlog-AspNetCore-CMS.git](https://github.com/mehmetyesildev/TechBlog-AspNetCore-CMS.git)
    ```
2.  **Configure Database:**
    Update the "ConnectionStrings" in `appsettings.json` to match your local SQL Server instance.
3.  **Update Database:**
    Open a terminal in the project directory and run:
    ```bash
    dotnet ef database update
    ```
4.  **Run the Application:**
    ```bash
    dotnet run
    ```

---

## 👨‍💻 Contact

* **Developer:** Mehmet Yeşil
* **Project:** ASP.NET Core Learning Path & Portfolio

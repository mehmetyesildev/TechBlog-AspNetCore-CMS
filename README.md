# 📢 Architecture Versions

This project demonstrates my ability to implement different authentication patterns. You can browse the source code for both versions via branches:

| Version | Architecture | Description | Link |
|---------|--------------|-------------|------|
| **v2.0 (Current)** | **ASP.NET Core Identity** | **Granular RBAC** (Admin/Moderator), **Privilege Escalation Protection**, Hashing, Lockout & 2FA ready. | [Browse Code](https://github.com/mehmetyesildev/TechBlog-AspNetCore-CMS/tree/main) |
| **v1.0 (Legacy)** | **Manual Cookie Auth** | Custom authentication built with `HttpContext.SignInAsync`. | [Browse Code](https://github.com/mehmetyesildev/TechBlog-AspNetCore-CMS/tree/v1-manual-cookie-auth) |

---

# 🚀 TechBlog - Modern ASP.NET Core CMS

![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-purple?style=flat&logo=dotnet) ![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-blue?style=flat&logo=nuget) ![Bootstrap](https://img.shields.io/badge/Bootstrap-5-orange?style=flat&logo=bootstrap) ![Security](https://img.shields.io/badge/Security-Identity%20%26%20RBAC-green) ![License](https://img.shields.io/badge/License-Non%20Commercial-red)

TechBlog is a dynamic content management system developed from scratch using **ASP.NET Core 8.0** architecture. Built upon the principles of **N-Tier Architecture**, the project has been enhanced with **Hierarchical Role-Based Authorization**, **AJAX-based interactions**, and a **Secure Admin Panel**.

---

## 🌟 Key Features & Technical Insights

### 🛡️ Advanced Security & Identity (v2.0 Update)
* **ASP.NET Core Identity:** Complete integration for secure user management, login, and registration.
* **Granular RBAC (Hierarchical Roles):**
    * **Admin:** Full access. Can manage Roles, Users, and assign other Admins.
    * **Moderator:** Can manage content and users but has **restricted privileges**.
    * **User:** Isolated environment. Can only view/edit their own profile.
* **Privilege Escalation Protection:**
    * **Backend Logic:** A specialized logic block prevents Moderators from assigning "Admin" or "Moderator" roles to others. This prevents unauthorized authority expansion.
    * **Frontend Security:** Dynamic Views automatically hide sensitive controls (e.g., "Admin" role checkbox, "Roles" menu link) based on the logged-in user's hierarchy.

### 🏗️ Backend & Architecture
* **Entity Framework Core (Code First):** Database entities designed as C# classes and managed via `Migrations`.
* **Repository Pattern:** Implemented to decouple business logic from data access, ensuring cleaner and testable code (`PostsRepository`).
* **Data Modeling:** Optimized One-to-Many (Author-Post, Post-Comment) and Many-to-Many (Post-Tag) relationships.
* **Seed Data:** Automated test data generation upon initial application startup.

### 🎨 Frontend & UI
* **Dynamic Navbar:** Menu items (Users/Roles) adjust visibility based on User Claims.
* **AJAX & jQuery:** Asynchronous comment submission and listing without page reloads.
* **ViewComponents:** Modular design for Sidebar, Popular Posts, and Tag Cloud.
* **State Management:** Visual badges for content status (Published/Draft) and user roles.

### ⚙️ Content Management System (CMS)
* **Rich Text Editor:** Integrated **TinyMCE** for HTML-formatted blog post creation.
* **File Management:** Server-side image upload handling via `IFormFile`.
* **Dynamic Filtering:** Content filtering by Tags or Categories via URL routing.

---

## 📸 v2.0 Security Showcase (Identity & RBAC)

This section demonstrates the **Hierarchical Security System** implemented in v2.0.

### 1. Granular Access Control (Admin vs. Moderator)
The most critical security feature: **Moderators cannot create Admins.** The system dynamically renders the UI based on privileges.

| **Admin Perspective (Full Control)** | **Moderator Perspective (Restricted)** |
|:---:|:---:|
| <img src="https://github.com/user-attachments/assets/0983b53d-bf85-41bb-a33f-f435072e53e8" width="100%" alt="Admin Edit View"> | <img src="https://github.com/user-attachments/assets/e631b368-fa29-477d-9bb9-3a79cea1171c" width="100%" alt="Moderator Edit View"> |
| *Admin can see and assign 'Admin' & 'Moderator' roles.* | *Critical roles are HIDDEN and backend-protected.* |

### 2. Identity Pages & Role Management

| **Secure Login Interface** | **Role Management (Admin Only)** |
|:---:|:---:|
| <img src="https://github.com/user-attachments/assets/c8760672-423d-4d6a-9c74-c23cd61c367f" width="100%" alt="Login Screen"> | <img src="https://github.com/user-attachments/assets/e4bc9820-5d9d-448f-a61e-5acc9177902b" width="100%" alt="Role Management"> |
| *Customized Identity forms.* | *Admins can manage system roles.* |

---

## 📸 General Application Screenshots

### 1. Modern Showcase (Home Page)
Responsive home page with pagination infrastructure.

<div align="center">
  <img src="https://github.com/user-attachments/assets/e9060a7e-aefc-4d18-8790-de15fc8cd66f" width="85%">
</div>

### 2. Content Creation Panel
TinyMCE editor and file upload mechanism.

<div align="center">
  <img src="https://github.com/user-attachments/assets/66e50e0c-d1d4-4d2f-9193-62042f4845fe" width="85%">
</div>

### 3. Interactive Comment System (AJAX)
Comments load and submit asynchronously.

<div align="center">
  <img src="https://github.com/user-attachments/assets/72763bb4-2b11-4c34-8020-9ccf496599a6" width="85%">
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

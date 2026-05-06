CoreDine is a high-performance ASP.NET Core MVC application designed to streamline restaurant operations, from menu management to real-time order tracking. This project was built to demonstrate proficiency in asynchronous patterns, secure role-based access, and the service-layer pattern.

🚀 Key Features
Role-Based Security: Distinct workspaces for Admin (Menu/Table management, Staff accounts) and Staff (Order fulfillment, Table status) using ASP.NET Core Identity.  

Menu & Table CRUD: Full management of restaurant resources with business logic validation (e.g., unique table numbers, price validation).  

Live Order System: Ability to create orders, take "snapshots" of menu prices at the time of order, and update order statuses through a workflow.  

Modern UI: A custom-styled dashboard using Bootstrap 5 and DM Sans typography for a professional, "SaaS" feel.  

🛠️ Tech Stack
Framework: .NET 8/9 ASP.NET Core MVC

Database: SQL Server via Entity Framework Core

Security: Microsoft Identity (Roles: Admin, Staff)

Frontend: Razor Pages, CSS3 (Custom Variables), JavaScript/jQuery

Patterns:

TAP (Task-based Asynchronous Pattern): Used for non-blocking database operations.  

Service Layer Pattern: Logic is decoupled from Controllers into dedicated Services.  

Dependency Injection: Clean management of service lifetimes.  

🏗️ Architecture Detail: The Service Layer
One of the core strengths of this project is the separation of concerns. Instead of putting database logic in the Controllers, I implemented a dedicated Service Layer:

MenuService: Handles menu availability and pricing logic.  

TableService: Manages dining room state and enforces table uniqueness.  

OrderService: Orchestrates complex logic including total calculations and price snapshotting.  

🚦 Getting Started
Prerequisites
Visual Studio 2022

.NET 8.0 or 9.0 SDK

SQL Server (LocalDB)

Installation
Clone the repository:

Bash
git clone https://github.com/hazem327/RestaurantManagementSystem.git
Update the Database:
Open the Package Manager Console and run:

Bash
Update-Database

🔐 Administrative Access
The system is pre-configured with a default administrator account for evaluation purposes.  

Admin Email: admin@coredine.com

  

Admin Password: Admin@1234

📝 Learning Objectives Reached
During this build, I focused on:

Implementing Async/Await properly to ensure thread scalability.  

Handling Complex Data Relationships (e.g., Many-to-Many via OrderItems).

Managing Application State through Middleware and Authentication filters.

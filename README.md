# Task Management

## Overview

The **Task Management Project** is a robust and flexible task management system designed to facilitate efficient organization and tracking of tasks and projects.  
Built with **ASP.NET Core** and modern **C# technologies**, it follows **Clean Architecture** principles to ensure scalability, maintainability, and high performance.

This project provides a comprehensive solution for managing tasks, projects, and boards — from creation and assignment to tracking their status and completion.

---

## ✨ Key Features

- **User Authentication** – User registration and login with **JWT**
- **Task Management** – Create, read, update, and delete tasks (CRUD).
- **Board Management**
  * Organize tasks into boards to visualize workflow.
  * Create boards within projects
  * create independent boards of any project
- **Project Management** – Group Boards into specific projects for better organization.
- **Centralized Error Handling** – Stable and consistent error management.
- **API Documentation** – Interactive API docs using Swagger/OpenAPI.

---

## 🏗 Architectural Structure (Clean Architecture)

The project adheres to **Clean Architecture** principles, ensuring a clear separation of concerns and high testability.

### 1. TaskManagement.API (Presentation Layer)
- Entry point of the application.  
- Handles HTTP requests/responses.  
- Contains **Controllers** and **Middlewares** (authentication, error handling).

### 2. TaskManagement.Application (Application Layer)
- Contains **business logic**.  
- Uses **CQRS** with **MediatR** (commands vs. queries).  
- Defines **DTOs** and **interfaces** for communication between layers.

### 3. TaskManagement.Domain (Domain Layer)
- Core of the application.  
- Contains **business entities, rules, enums, and exceptions**.  
- Independent of any external technology.

### 4. TaskManagement.Infrastructure (Infrastructure Layer)
- Handles **database access** and external integrations.  
- Implements contracts from the Domain layer.  
- Manages persistence, migrations, and external services.

---

## 🛠 Technologies Used

- **.NET 9** – Cross-platform framework.  
- **C#** – Primary language.  
- **ASP.NET Core** – For building the Web API.  
- **MediatR** – CQRS implementation.  
- **Entity Framework Core** – ORM for database access.  
- **JWT (JSON Web Tokens)** – Secure authentication.  
- **Swagger/OpenAPI** – API documentation.  
- **SQL Server** – Relational database system.

---

## 🚀 Getting Started

### Prerequisites
- [.NET SDK 9](https://dotnet.microsoft.com/download)  
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)  
- [Git](https://git-scm.com/downloads)  

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/Mostafa-Y3sser/TaskManagementProject.git
   cd TaskManagementProject/TaskManagement.API
    ```


2. **Configure the database**
* Open appsettings.json in TaskManagement.API
* Update the Connection String with your SQL Server details
 ```bash
dotnet ef database update
 ```

3. **Run the API**
  ```bash
dotnet run
 ```

The API will be running at:
👉 [https://localhost:44371](https://localhost:44371/)

API documentation available at:

* `/openapi`

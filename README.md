Clinic Management System - ASP.NET Core 8

A comprehensive clinic management RESTful API built with ASP.NET Core 8 and Entity Framework Core, providing full CRUD operations for patients, doctors, appointments, bills, medications, and prescriptions, with role-based authorization and clean RESTful design.

Features

User registration and management for Patients, Doctors, and Admins with role-based permissions.

JWT Authentication + Refresh Tokens for secure sessions.

Full CRUD on Patients, Doctors, Appointments, Bills, Medications, and Prescriptions.

Appointment management with multiple statuses: Confirm, Complete, NoShow, Reschedule, Cancel.

Smart filtering: Doctors can only see their own appointments; Patients can only see theirs.

From prescriptions, entities are automatically added to Records and Prescription-Medication tables.

Email confirmation and password reset functionality.

Transactions for saving multiple related entities safely.

Fluent Validation for model validation.

Role-based restrictions to prevent unauthorized access.

Person entity integrated with Patient and Doctor entities.

Seed Data populated during startup.

Unit of Work & Generic Repository pattern implemented for clean data access.

Postman Test Collection included for easy API testing.

Unified API response using a Result class.

Technologies Used

ASP.NET Core 8 Web API

Entity Framework Core (Code First)

SQL Server

JWT Authentication + Refresh Tokens

Role-Based Authorization

Fluent Validation

Unit of Work & Generic Repository

Transactions & Seed Data

Swagger UI & OpenAPI

Postman Collection (included in project)

How to Run Locally

Clone the project:

git clone https://github.com/<USERNAME>/ClinicApp.git

Navigate to the project folder:

cd ClinicApp

Create or update appsettings.json from appsettings.example.json and set your connection string and JWT key.

Apply database migrations:

dotnet ef database update

Run the API:
dotnet run

Online Store API
This is a full-featured ASP.NET Core Web API project for managing an online store. It demonstrates clean architecture, separation of concerns, and modern backend development practices.
Features
JWT Authentication and Role-based Authorization
Product, Category, and Order management
Validation and error handling with ServiceResult
Entity Framework Core + SQL Server
Dependency Injection
DTOs and layered architecture
Technologies Used
ASP.NET Core 8 | Entity Framework Core | SQL Server | AutoMapper | FluentValidation | JWT Authentication
How to Run Locally
1️⃣ Download the Project:
You can either clone or download it manually:

Option 1 (recommended):
In Git Bash, run:
git clone https://github.com/chikolaid6-cell/E-CommerceAPI.git
Option 2: Click Code → Download ZIP, then extract it. 2️⃣ Navigate to the Project Folder: cd E-CommerceAPI 3️⃣ Create or Update appsettings.json: Copy from appsettings.example.json and set your connection string and JWT key. 4️⃣ Apply Database Migrations: dotnet ef database update 5️⃣ Run the API: dotnet run

Collaboration
This API is open for Frontend Developers who want to practice integration.

If you build a frontend (React / Angular / Vue) for this project, feel free to share it in the Issues or tag me — I’d love to see it!

Chikh Oulad Laid Backend Developer — ASP.NET Core LinkedIn | GitHub

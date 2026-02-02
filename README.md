# Problem 
Managing clinic operations often leads to inconsistent data, unauthorized access to sensitive information, and complex transactional workflows (appointments, prescriptions, billing).

This project was built to explore how to design a secure, scalable backend API that enforces role-based access, maintains data integrity across multiple related entities, and supports real-world clinic workflows.

# Why this architecture
- Used Role-Based Authorization to ensure doctors and patients only access their own data.
- Implemented Transactions to guarantee consistency when creating prescriptions and related records.
- Applied Unit of Work to manage complex operations across multiple repositories safely.
- Used Fluent Validation to centralize and enforce business rules outside controllers.

# What I learned
- Designing APIs beyond CRUD.
- Handling real-world authorization scenarios.
- Managing relational data consistency.
- Structuring backend projects for maintainability.

# Clinic Management System - ASP.NET Core 8

A comprehensive clinic management RESTful API built with **ASP.NET Core 8** and **Entity Framework Core**, providing full CRUD operations for patients, doctors, appointments, bills, medications, and prescriptions, with role-based authorization and clean RESTful design.

---

## Features

- User registration and management for Patients, Doctors, and Admins with role-based permissions.
- JWT Authentication + Refresh Tokens for secure sessions.
- Full CRUD on Patients, Doctors, Appointments, Bills, Medications, and Prescriptions.
- Appointment management with multiple statuses: Confirm, Complete, NoShow, Reschedule, Cancel.
- Smart filtering: Doctors can only see their own appointments; Patients can only see theirs.
- From prescriptions, entities are automatically added to **Records** and **Prescription-Medication** tables.
- Email confirmation and password reset functionality.
- Transactions for saving multiple related entities safely.
- Fluent Validation for model validation.
- Role-based restrictions to prevent unauthorized access.
- Person entity integrated with Patient and Doctor entities.
- Seed Data populated during startup.
- Unit of Work & Generic Repository pattern implemented for clean data access.
- Postman Test Collection included for easy API testing.
- Unified API response using a **Result class**.

---

## Technologies Used

- ASP.NET Core 8 Web API  
- Entity Framework Core (Code First)  
- SQL Server  
- JWT Authentication + Refresh Tokens  
- Role-Based Authorization  
- Fluent Validation  
- Unit of Work & Generic Repository  
- Transactions & Seed Data  
- Swagger UI & OpenAPI  
- Postman Collection (included in project)

---

## Test the API:
- http://clinicappapi.runasp.net/swagger/index.html


## How to Run Locally

Follow these steps to set up and run the project on your local machine:

1️⃣ **Clone the project:**  

git clone https://github.com/chikh97laid/ClinicApp.git

2️⃣ Navigate to the project folder:

cd ClinicApp

3️⃣ Create or update appsettings.json:

Copy from appsettings.example.json

Set your database connection string and JWT key.

4️⃣ Apply database migrations:
dotnet ef database update

5️⃣ Run the API:
dotnet run

Chikh Oulad Laid Backend Developer — ASP.NET Core 
[LinkedIn](www.linkedin.com/in/chikhouladlaid) 
|
[GitHub](https://github.com/chikh97laid)

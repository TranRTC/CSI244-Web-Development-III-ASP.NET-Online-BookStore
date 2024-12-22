# Online Bookstore Management System

A web application developed as a capstone project for **CSI-244: ASP.NET Programming**, showcasing the use of **ASP.NET MVC Framework** and **Entity Framework (Code-First)** to manage an online bookstore. The project includes features such as CRUD operations, authentication/authorization, and a SQL Server database.

---

## Purpose

This project was chosen to:
- Explore personal interest in e-commerce platforms.
- Gain hands-on experience building a real-world **ASP.NET MVC** application.
- Understand the **Model-View-Controller (MVC)** design pattern and implement database migrations using **Entity Framework**.

---

## Features

- **Book Management**: CRUD operations for books.
- **Author Management**: Manage authors associated with books.
- **Customer Management**: CRUD operations for customer accounts.
- **Authentication/Authorization**:
  - User login functionality with role-based access control.
  - Secured endpoints for authorized roles only.
- **Custom User Interface**:
  - Customized UI for the Book Index page beyond default scaffolding.
- **Database Relationships**:
  - Author ↔ Book: One-to-Many
  - Book ↔ OrderItem: One-to-Many
  - OrderItem ↔ Order: Many-to-One
  - Order ↔ Customer: Many-to-One

---

## Technologies Used

- **Frontend**:
  - ASP.NET Razor Views
  - Bootstrap for responsive design
- **Backend**:
  - ASP.NET MVC Framework
  - C# for server-side logic
- **Database**:
  - SQL Server
  - Entity Framework (Code-First approach)
- **Tools**:
  - Visual Studio for development
  - Git and GitHub for version control

---

## Installation and Setup

### Prerequisites
- **Visual Studio** with ASP.NET and database workloads installed.
- **SQL Server** or any compatible database engine.
- **.NET Framework** installed.



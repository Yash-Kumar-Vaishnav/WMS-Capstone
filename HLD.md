# High-Level Design (HLD) - Workforce Management System (WMS)

## 1. Introduction
The Workforce Management System (WMS) is a comprehensive multi-tier web application designed to manage employees, departments, client projects, attendance, and leaves. 

## 2. Architecture Overview
The system follows a clean, decoupled architecture:
- **Frontend**: Angular 17 SPA, utilizing Angular Material, Reactive Forms, and Role-Based Guards.
- **Backend API**: ASP.NET Core 8 Web API, adhering to RESTful principles.
- **Database**: SQL Server accessed via Entity Framework Core.
- **Security**: JWT-based Authentication with Role-Based Access Control (Admin, Manager, Employee) and BCrypt Password Hashing.

## 3. Core Modules
- **Employee Management**: CRUD with data validation and uniqueness constraints.
- **Leave Management**: Hierarchical approval workflow.
- **Attendance**: Real-time Check-In/Check-Out with automated hours calculation.
- **Project Allocation**: Matrix structure assigning employees to client projects.
- **Audit Logging**: Automatic interceptors inside Entity Framework tracking modifications.

## 4. Security & Isolation
Data isolation ensures that 'Employee' roles only interact with their subset of data, governed explicitly on the server-side via JWT `ClaimTypes.NameIdentifier`.

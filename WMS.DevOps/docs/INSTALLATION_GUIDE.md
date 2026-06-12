# Installation Guide

## Prerequisites
- .NET 8 SDK
- Node.js (v18+)
- Angular CLI (v17)
- SQL Server (LocalDB or full instance)

## Backend Setup (WMS.API)
1. Navigate to the `WMS.API` project folder.
2. Ensure the `appsettings.json` connection string points to your SQL Server instance.
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=WMSDB;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```
3. Open a terminal and run Entity Framework migrations:
   ```bash
   dotnet ef database update
   ```
4. Run the API:
   ```bash
   dotnet run
   ```
   The API will be available at `https://localhost:5001`.

## Frontend Setup (WMS.Frontend)
1. Navigate to the `WMS.Frontend` folder.
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the Angular development server:
   ```bash
   ng serve
   ```
4. Access the application at `http://localhost:4200`.

## Default Credentials
- **Username:** admin
- **Password:** Admin@123
- **Role:** Admin

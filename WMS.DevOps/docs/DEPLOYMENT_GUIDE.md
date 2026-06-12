# Deployment Guide

## Azure DevOps CI/CD

The application is configured for continuous integration and deployment using Azure Pipelines.

### 1. Azure Pipeline Configuration
The pipeline definition is located at `WMS.DevOps/azure-pipelines.yml`.
It consists of two main stages:
- **Build Stage**: Restores dependencies, builds the .NET Solution, runs xUnit tests, and builds the Angular application.
- **Deploy Stage**: Deploys the built artifacts to Azure App Service (for the API) and Azure Static Web Apps or Azure Storage (for the Frontend).

### 2. Required Variables in Azure DevOps
Ensure the following variables are configured in your Azure DevOps Pipeline environment:
- `AzureSubscription`: Your Azure Service Connection name.
- `WebAppName`: The name of the target Azure App Service.
- `ConnectionStrings__DefaultConnection`: Production database connection string (set in Azure App Service Configuration).
- `JwtSettings__SecretKey`: Production JWT secret.

### 3. Manual Deployment (IIS / Windows Server)
#### Backend:
1. Run `dotnet publish WMS.API -c Release -o ./publish`.
2. Copy the `publish` folder to your IIS Server.
3. Configure IIS to point to the `publish` folder and install the ASP.NET Core Hosting Bundle.
4. Update `appsettings.Production.json` with your production DB credentials.

#### Frontend:
1. Run `ng build --configuration production`.
2. Copy the `dist/wms-frontend` contents to your IIS web root or a static hosting provider.
3. If using IIS, ensure URL Rewrite is configured to redirect all requests to `index.html`.

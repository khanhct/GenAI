# Electrolux.BFF.ComparePage

Backend-for-Frontend (BFF) service for the Compare Page Slice, built using Azure Functions v4 and .NET 8.

## Project Structure

```
Electrolux.BFF.ComparePage/
├── Constants/
├── Dtos/
├── Functions/
│   └── HealthCheckFunction.cs
├── Middleware/
│   └── AuthorizationMiddleware.cs
├── Models/
├── Services/
│   ├── Handlers/
│   └── Interfaces/
├── Program.cs
├── host.json
└── Electrolux.BFF.ComparePage.csproj

Electrolux.BFF.ComparePage.Tests/
├── Functions/
│   └── HealthCheckFunctionTests.cs
└── Electrolux.BFF.ComparePage.Tests.csproj
```

## Prerequisites

- .NET 8 SDK
- Azure Functions Core Tools v4
- Visual Studio 2022 or VS Code with C# extension
- Azure subscription for deployment

## Local Development Setup

1. Clone the repository:
   ```bash
   git clone <repository-url>
   ```

2. Install dependencies:
   ```bash
   dotnet restore
   ```

3. Create a `local.settings.json` file in the Electrolux.BFF.ComparePage directory:
   ```json
   {
     "IsEncrypted": false,
     "Values": {
       "AzureWebJobsStorage": "UseDevelopmentStorage=true",
       "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
       "T1Api:BaseUrl": "https://api.t1.com",
       "AuthApi:BaseUrl": "https://auth.t1.com"
     },
     "ConnectionStrings": {
       "RedisConnection": "your-redis-connection-string"
     }
   }
   ```

4. Run the project:
   ```bash
   cd Electrolux.BFF.ComparePage
   func start
   ```

5. Run tests:
   ```bash
   dotnet test
   ```

## Features

- Health Check endpoint at `GET /api/health`
- Azure Application Insights integration
- Redis Cache integration
- Clean Architecture implementation
- Authentication and Authorization middleware
  - Supports B2B and D2C user scenarios
  - Integrates with T1 API for access rights
  - Handles redirects based on user context

## Development Guidelines

1. Follow Clean Architecture principles
2. Write unit tests for new functionality
3. Use dependency injection
4. Document new endpoints and features
5. Follow existing code style and patterns

## Deployment

The service is deployed to Azure Functions using Azure DevOps pipelines. Contact the DevOps team for repository and deployment setup.

## Monitoring

- Application Insights is configured for monitoring and logging
- Health check endpoint for service status verification

## Contributing

1. Create a feature branch from `main`
2. Make your changes
3. Write/update tests
4. Create a pull request
5. Get code review and approval

## Support

For any issues or questions, contact the development team or create an issue in the repository.
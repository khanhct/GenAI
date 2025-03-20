# Electrolux.BFF.ComparePage

Backend-for-Frontend (BFF) service for the Compare Page functionality. This service acts as an intermediary between the frontend and backend services, providing optimized and aggregated data for the product comparison feature.

## Features

- Product comparison data retrieval
- Market and language-specific content handling
- Response caching for improved performance
- Correlation ID tracking for request tracing
- Error handling and logging

## Project Structure

```
Electrolux.BFF.ComparePage/
│── Constants/           # Constant values and configuration
│── Dtos/               # Data Transfer Objects
│── Functions/          # Azure Functions
│── Models/             # Domain models
│── Properties/         # Project properties
│── Services/           # Business logic implementation
    ├── Handlers/       # Request handlers
    └── Interfaces/     # Service interfaces
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Azure Functions Core Tools
- Visual Studio 2022 or VS Code

### Local Development

1. Clone the repository
2. Initialize and update git submodules:
   ```bash
   git submodule init
   git submodule update
   ```
3. Restore dependencies:
   ```bash
   dotnet restore
   ```
4. Run the project:
   ```bash
   dotnet run
   ```

### Testing

Run the tests using:
```bash
dotnet test
```

## API Endpoints

### Compare Products

```http
POST /api/compare
```

Headers:
- `x-correlation-id`: Request correlation ID
- `x-market`: Market code
- `x-language`: Language code

Request body:
```json
{
  "productIds": ["string"],
  "market": "string",
  "language": "string"
}
```

## Contributing

1. Create a feature branch
2. Make your changes
3. Run tests
4. Submit a pull request

## License

Proprietary - Electrolux
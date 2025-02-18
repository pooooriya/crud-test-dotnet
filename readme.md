# Customer Management CRUD Application

This is a clean architecture implementation of a customer management system using .NET 7, following CQRS pattern and Domain-Driven Design principles.

## Technologies Used

- .NET 7
- Blazor WebAssembly
- Entity Framework Core
- SQL Server
- MediatR for CQRS
- FluentValidation
- Google LibPhoneNumber
- Docker & Docker Compose
- xUnit for Testing

## Features

- Clean Architecture implementation
- CQRS pattern with MediatR
- Domain-Driven Design
- Test-Driven Development
- Behavior-Driven Development
- Comprehensive validation including:
  - Phone number validation using Google LibPhoneNumber
  - Email validation
  - Bank account number validation
  - Unique customer constraints (FirstName + LastName + DateOfBirth)
  - Unique email constraint
- Docker support with SQL Server integration
- Blazor WebAssembly UI

## Project Structure

- `Mc2.CrudTest.Domain`: Contains entities, interfaces, and domain logic
- `Mc2.CrudTest.Application`: Contains application logic, CQRS commands/queries
- `Mc2.CrudTest.Infrastructure`: Contains data access and external service implementations
- `Mc2.CrudTest.Presentation`: Contains the Blazor WebAssembly UI
- `Mc2.CrudTest.UnitTests`: Contains unit tests
- `Mc2.CrudTest.AcceptanceTests`: Contains acceptance tests

## Getting Started

### Prerequisites

- .NET 7 SDK
- Docker Desktop
- Visual Studio 2022 or VS Code

### Running with Docker

1. Clone the repository
2. Navigate to the root directory
3. Run the following commands:

```bash
docker-compose build
docker-compose up
```

The application will be available at:

- API: http://localhost:5000
- Blazor UI: http://localhost:5000

### Running Locally

1. Clone the repository
2. Navigate to the root directory
3. Update the connection string in `appsettings.json` if needed
4. Run the following commands:

```bash
dotnet restore
dotnet build
cd Mc2.CrudTest.Presentation/Server
dotnet run
```

### Running Tests

```bash
dotnet test
```

## Design Decisions

1. **Clean Architecture**: Implemented to ensure separation of concerns and maintainability
2. **CQRS Pattern**: Used to separate read and write operations
3. **Domain-Driven Design**: Implemented to focus on the core domain logic
4. **Validation Strategy**:
   - Phone numbers are validated using Google LibPhoneNumber
   - Unique constraints are enforced at both application and database levels
   - All validations are centralized in the Application layer
5. **Storage Optimization**:
   - Phone numbers are stored as varchar(50) to minimize space
   - Appropriate indexes are created for frequent queries
6. **Docker Support**:
   - Multi-stage build for optimal image size
   - Includes SQL Server container
   - Volume persistence for database data

## Contributing

1. Create a feature branch
2. Commit your changes
3. Push to the branch
4. Create a Pull Request

## License

This project is licensed under the MIT License.

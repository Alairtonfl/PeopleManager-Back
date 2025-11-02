# PeopleManager API

A robust .NET 8.0 Web API for managing people records with authentication, versioning, and clean architecture principles.

## 🚀 Technologies

- **.NET 8.0**
- **Entity Framework Core 9.0**
- **AutoMapper**
- **JWT Authentication**
- **Swagger/OpenAPI**
- **API Versioning**

## 📁 Project Structure

The solution follows Clean Architecture principles with the following projects:

```
PeopleManager/
├── PeopleManager.API           # Web API layer
├── PeopleManager.Application   # Application logic layer
├── PeopleManager.Domain        # Domain entities and business rules
├── PeopleManager.Infrastructure# Data access and external services
└── PeopleManager.Tests        # Unit tests
```

### Project Descriptions

- **PeopleManager.API**: Entry point of the application, contains controllers, middleware configurations, and API models
- **PeopleManager.Application**: Contains application services, DTOs, interfaces, and validators
- **PeopleManager.Domain**: Contains domain entities, enums, and business exceptions
- **PeopleManager.Infrastructure**: Implements data access, repositories, and security features
- **PeopleManager.Tests**: Contains unit tests for services

## 🔑 Features

- API versioning (v1, v2)
- JWT Authentication
- Cookie-based token storage
- CRUD operations for people management
- Custom exception handling
- Input validation
- AutoMapper for object mapping
- Unit testing

## 🛣️ API Endpoints

### Authentication (v1)

```
POST api/v1/Auth/authenticate  # Authenticate user and get JWT token
GET  api/v1/Auth/me           # Get current authenticated user info
```

### People Management (v1)

```
POST   api/v1/People/create          # Create a new person
GET    api/v1/People/get-all         # Get all people
GET    api/v1/People/get-by-cpf/{cpf}# Get person by CPF
DELETE api/v1/People/delete/{id}     # Delete person by ID
PUT    api/v1/People/update/{id}     # Update person by ID
GET    api/v1/People/get-by-id/{id}  # Get person by ID
```

## 🔒 Security

- JWT (JSON Web Token) authentication
- Secure cookie storage for tokens
- HTTPS enabled
- Authorization using [Authorize] attribute
- Password hashing and security best practices

## 🛠️ Prerequisites

- .NET 8.0 SDK
- SQL Server (or your configured database)
- Visual Studio 2022 or VS Code

## 🚀 Getting Started

1. Clone the repository
```powershell
git clone https://github.com/Alairtonfl/PeopleManager-Back.git
```

2. Navigate to the project directory
```powershell
cd PeopleManager
```

3. Restore dependencies
```powershell
dotnet restore
```

4. Update the database (ensure connection string is configured in appsettings.json)
```powershell
dotnet ef database update
```

5. Run the application
```powershell
dotnet run --project PeopleManager/PeopleManager.API.csproj
```

## 📝 Configuration

The application uses `appsettings.json` for configuration. Make sure to update the following settings:

- Database connection string
- JWT settings
- CORS policies
- Logging configuration

## 🧪 Running Tests

```powershell
dotnet test PeopleManager.Tests/PeopleManager.Tests.csproj
```

## 🏗️ Architecture

The project follows Clean Architecture principles with:

- Separation of concerns
- Dependency injection
- Repository pattern
- Unit of Work pattern
- Service layer pattern
- SOLID principles

## 📚 API Documentation

API documentation is available through Swagger UI at:
```
https://localhost:5001/swagger
```

Different API versions can be accessed through the Swagger UI version selector.


## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

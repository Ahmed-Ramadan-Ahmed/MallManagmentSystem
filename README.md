# Mall Management System

A comprehensive .NET 8.0-based web application for managing mall operations, including tenant management, facility booking, and maintenance tracking.

## Technologies Used

- .NET 8.0
- Entity Framework Core 8.0.2
- SQL Server
- JWT Authentication
- Swagger/OpenAPI
- iText7 (PDF generation)
- MailKit (Email functionality)
- Twilio (SMS notifications)

## Project Structure

- `Controllers/` - API endpoints and request handling
- `Services/` - Business logic implementation
- `Interfaces/` - Service contracts and abstractions
- `Models/` - Domain entities and data models
- `DTOs/` - Data Transfer Objects
- `Data/` - Database context and configurations
- `Migrations/` - Database migration files

## Features

- User authentication and authorization
- Tenant management
- Facility booking system
- Maintenance request tracking
- PDF report generation
- Email notifications
- SMS notifications

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
```bash
git clone [repository-url]
```

2. Navigate to the project directory
```bash
cd MallManagmentSystem
```

3. Restore dependencies
```bash
dotnet restore
```

4. Update the connection string in `appsettings.json`

5. Run database migrations
```bash
dotnet ef database update
```

6. Start the application
```bash
dotnet run
```

The application will be available at `https://localhost:5001` and `http://localhost:5000`

## API Documentation

API documentation is available through Swagger UI at `/swagger` when running the application.

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details
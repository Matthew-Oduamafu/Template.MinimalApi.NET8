# .NET 8.0 Minimal API Project Template

This project is a .NET 8.0 Minimal API application that follows best practices in software development, including the use of the Repository and Service patterns, custom extensions, custom middlewares, and exception handling. It also includes a configured `ApplicationDbContext` for data access.

## Project Structure

The project is structured into several folders, each serving a specific purpose:

- `Data`: Contains the `ApplicationDbContext` class, which is the main class for interacting with the database using Entity Framework Core.
- `Repositories`: Contains interfaces and implementations for the Repository pattern. The repository pattern is used to decouple the business logic and the data access layers in your application.
- `Services`: Contains interfaces and implementations for the Service pattern. The service layer is an abstraction over the domain logic.
- `Models`: Contains data transfer objects (DTOs) and response models.
- `Middlewares`: Contains custom middleware classes for handling cross-cutting concerns such as logging, exception handling, etc.
- `Extensions`: Contains custom extension methods that add new functionality to existing .NET types.

## Key Features

- **.NET 8.0 Minimal API**: This project uses the new Minimal API feature introduced in .NET 8.0, which allows for creating HTTP APIs with minimal coding and configuration.
- **Entity Framework Core**: The project uses Entity Framework Core as an Object-Relational Mapper (ORM) for .NET. It supports LINQ queries, change tracking, updates, and schema migrations.
- **Repository Pattern**: The repository pattern is used to decouple the business logic and the data access layers in your application.
- **Service Pattern**: The service layer is an abstraction over the domain logic. It interacts with the repositories to get the data, applies the business rules on it, and then returns the result.
- **Custom Middlewares**: The project includes custom middleware for handling cross-cutting concerns such as logging, exception handling, etc.
- **Exception Handling**: The project includes a global exception handling mechanism to catch and handle exceptions in a centralized place.

## Getting Started

To get started with this project, you need to have .NET 8.0 SDK installed on your machine. Once you have that, you can clone this repository and open it in your favorite IDE.

## Running the Project

To run the project, navigate to the project directory in your terminal and run the following command:

```bash
dotnet run

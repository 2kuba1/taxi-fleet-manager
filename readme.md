# Taxi Fleet Manager

> This project provides a comprehensive backend API for managing a taxi fleet, built using ASP.NET Core. It facilitates user authentication, manages various domain entities such as users (drivers, administrators), cars, car fleets, work shifts, and shift reports, persisting data in a PostgreSQL database.

![GitHub stars](https://img.shields.io/github/stars/2kuba1/taxi-fleet-manager?style=social)

## ✨ Key Features

*   **User Authentication & Authorization**: Secure registration, login, and refresh token mechanisms using JWT.

## 🤝 TODO

*   **User Management**: Defines users (drivers, administrators) with properties like email, login, phone number, kilometer rate, and contract type, assignable to specific roles and teams.
*   **Fleet Management**: Manages individual cars with detailed information including brand, model, license plate, VIN, inspection dates, and insurance renewal dates, organized into car fleets.
*   **Work Shift & Reporting**: Functionality to track work shifts and generate shift reports.
*   **Team Organization**: Users can be assigned to teams, allowing for logical grouping (e.g., by city branch).
*   **Data Persistence**: Robust data storage and retrieval using PostgreSQL.
*   **settlement with employees**: Users can track their killometers driven and card payments sum and than settle with team owner


## 🛠️ Tech Stack

*   **Framework**: ASP.NET Core
*   **Runtime**: .NET 10.0
*   **ORM**: Entity Framework Core
*   **Database**: PostgreSQL
*   **Authentication**: JWT (JSON Web Tokens)
*   **Containerization**: Docker & Docker Compose
*   **Command/Query Pattern**: Cortex.Mediator
*   **Identity Management**: ASP.NET Core Identity
*   **API Documentation**: Swagger/OpenAPI

## 🚀 Installation

To set up and run the Taxi Fleet Manager API locally using Docker Compose, follow these steps:

1.  **Prerequisites**: Ensure you have Docker and Docker Compose installed on your system.
2.  **Environment Variables**: Create a `.env` file in the project's root directory (where `compose.yaml` is located). Populate it with the following environment variables:
    ```ini
    DB_USER=<your_database_user>
    DB_PASSWORD=<your_database_password>
    DB_DATABASE=<your_database_name>

    JWT_SECRET=<a_strong_secret_key_for_jwt>
    JWT_ISSUER=<your_jwt_issuer>
    JWT_AUDIENCE=<your_jwt_audience>
    JWT_ACCESS_TOKEN_EXPIRATION=<access_token_expiration_in_minutes>
    JWT_REFRESH_TOKEN_EXPIRATION=<refresh_token_expiration_in_days>

    SEED_INITIAL_OWNER_PASSWORD=<initial_admin_password>
    SEED_INITIAL_OWNER_EMAIL=<initial_admin_email>
    SEED_INITIAL_OWNER_LOGIN=<initial_admin_login>
    SEED_INITIAL_OWNER_PHONE=<initial_admin_phone_number>
    SEED_INITIAL_OWNER_AREA_CODE=<initial_admin_phone_area_code>
    SEED_INITIAL_OWNER_FIRST_NAME=<initial_admin_first_name>
    SEED_INITIAL_OWNER_LAST_NAME=<initial_admin_last_name>
    ```
3.  **Run with Docker Compose**: Navigate to the project's root directory in your terminal and execute the following command:
    ```bash
    docker compose up -d
    ```
    This command will build the API service image, pull the PostgreSQL image, create and start both containers, and run them in detached mode.

## Usage

Once the services are running:

*   The API endpoints will be accessible at `http://localhost:5000` (HTTP) and `https://localhost:5001` (HTTPS).
*   During development, the API documentation (Swagger UI) can be accessed by navigating to `http://localhost:5000/` in your web browser. This interface allows you to explore the available endpoints and interact with the API.

## 🔧 How It Works

The Taxi Fleet Manager backend is structured using a clean, layered architecture, ensuring separation of concerns and maintainability.

1.  **API Layer (`src/API`)**: This is the entry point of the application, responsible for handling HTTP requests. It configures ASP.NET Core, integrates authentication (JWT Bearer), enables API documentation via Swagger, and registers services from other layers. Requests are mapped to specific endpoints, such as `AuthEndpoints`, which dispatch commands and queries.
2.  **Application Layer (`src/Application`)**: Contains the core business logic in the form of commands and queries (leveraging Cortex.Mediator). It orchestrates interactions between the domain and persistence layers, validating input and executing business rules. This layer defines contracts (interfaces) for persistence and infrastructure services.
3.  **Domain Layer (`src/Domain`)**: The heart of the application, defining all core business entities (`User`, `Car`, `Role`, `ShiftReport`, `WorkShift`, `Team`, `RefreshToken`) and value objects (`LicensePlate`, `KilometerRate`, `PhoneNumber`). It encapsulates domain-specific rules and behaviors, ensuring data integrity and consistency through factory methods (e.g., `User.Create`, `Car.Create`) and exceptions.
4.  **Infrastructure Layer (`src/Infrastructure`)**: Implements external services and concerns that are not core to the business domain, such as `EmailService`.
5.  **Persistence Layer (`src/Persistence`)**: Responsible for data access and storage. It implements the persistence contracts defined in the Application layer using Entity Framework Core and a `AppDbContext` to interact with the PostgreSQL database. This layer handles database migrations, data seeding (e.g., `UserTeamRoleSeeder`), and manages transactions (`IUnitOfWork`).

The entire application stack is containerized using Docker and Docker Compose. The `compose.yaml` file defines two main services: an `api` service (the ASP.NET Core application) and a `database` service (a PostgreSQL instance). This setup ensures a consistent and isolated development and deployment environment. Upon startup, the `api` service connects to the `database` service, runs any pending Entity Framework migrations, and seeds initial data into the database.
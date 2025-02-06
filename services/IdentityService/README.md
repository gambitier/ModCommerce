# Identity Service

This service handles user authentication and authorization for the system.

## Prerequisites

- .NET 9.0 SDK
- Entity Framework Core tools (installed via `make install-ef`, see Configuration section)
- PostgreSQL database
  
## Make Commands

Make is a build automation tool that simplifies executing commands through a Makefile. It's commonly pre-installed on Linux and macOS systems.

### Installing Make

- **Windows**: 
  - Install via Chocolatey: `choco install make`
  - Or install via Scoop: `scoop install make`
  
- **macOS**:
  - Comes pre-installed
  - Or install via Homebrew: `brew install make`

- **Linux**:
  - Ubuntu/Debian: `sudo apt-get install make`
  - RHEL/CentOS: `sudo yum install make`

### Available Commands

Run `make help` to see all available commands:

## Configuration

### 1. Install Entity Framework Tools

```bash
make install-ef
```
### 2. Configure appsettings files

The service uses two appsettings files:

- `appsettings.json`: Contains the base configuration structure with empty values. This file should be committed to source control.
- `appsettings.Development.json`: Contains development-specific values and secrets. This file should NOT be committed to source control.

Update `appsettings.Development.json` (or create environment-specific files like `appsettings.Production.json`) with your configuration:


## Database Management

The service uses Entity Framework Core for database operations. The following commands are available:

```bash
# Create a new migration
make add-migration name=YourMigrationName

# Apply migrations to the database
make update-database

# List all migrations
make list-migrations

# Remove the last migration
make remove-migration

# Generate SQL script
make generate-script

# Drop the database (use with caution!)
make drop-database
```

## Running the Service

1. Ensure all configurations are set
2. Apply database migrations:
   ```bash
   make update-database
   ```
3. Start the service:
   ```bash
   dotnet run --project src/IdentityService.API
   ```

## API Documentation

Once the service is running, you can access the API documentation at: [https://localhost:7024/scalar](https://localhost:7024/scalar)

The API documentation is powered by Scalar and provides interactive documentation for all available endpoints.

## Project Structure

- **IdentityService.API**: Main API project with controllers and configuration
- **IdentityService.Application**: Business logic and application services
- **IdentityService.Infrastructure**: Data access and external service integrations
- **IdentityService.Domain**: Core domain models, interfaces and business rules

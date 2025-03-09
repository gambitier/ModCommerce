# Variables
STARTUP_PROJECT := src/UserService.API
INFRASTRUCTURE_PROJECT := src/UserService.Infrastructure

# Colors for terminal output
GREEN := \033[0;32m
NC := \033[0m # No Color
INFO := ${GREEN}==${NC}

# Default target
.DEFAULT_GOAL := help

# Install EF Core tools
install-ef:
	@echo "${INFO} Installing Entity Framework Core tools..."
	dotnet tool install --global dotnet-ef

# Update EF Core tools
update-ef:
	@echo "${INFO} Updating Entity Framework Core tools..."
	dotnet tool update --global dotnet-ef

# Add new migration
# Usage: make add-migration name=YourMigrationName
add-migration:
	@if [ "$(name)" = "" ]; then \
		echo "Error: Migration name is required. Usage: make add-migration name=YourMigrationName"; \
		exit 1; \
	fi
	@echo "${INFO} Creating new migration: $(name)"
	dotnet ef migrations add $(name) --project $(INFRASTRUCTURE_PROJECT) --startup-project $(STARTUP_PROJECT) --output-dir Persistence/Migrations

# Remove last migration
remove-migration:
	@echo "${INFO} Removing last migration..."
	dotnet ef migrations remove --project $(INFRASTRUCTURE_PROJECT) --startup-project $(STARTUP_PROJECT)

# List all migrations
list-migrations:
	@echo "${INFO} Listing all migrations..."
	dotnet ef migrations list --project $(INFRASTRUCTURE_PROJECT) --startup-project $(STARTUP_PROJECT)

# Update database to latest migration
update-database:
	@echo "${INFO} Updating database to latest migration..."
	dotnet ef database update --project $(INFRASTRUCTURE_PROJECT) --startup-project $(STARTUP_PROJECT)

# Generate SQL script
generate-script:
	@echo "${INFO} Generating SQL script..."
	dotnet ef migrations script --project $(INFRASTRUCTURE_PROJECT) --startup-project $(STARTUP_PROJECT)

# Drop the database
drop-database:
	@echo "${INFO} Dropping database..."
	dotnet ef database drop --project $(INFRASTRUCTURE_PROJECT) --startup-project $(STARTUP_PROJECT) --force

# Help command
help:
	@echo "Available commands:"
	@echo "  make install-ef                       - Install Entity Framework Core tools"
	@echo "  make update-ef                        - Update Entity Framework Core tools"
	@echo "  make add-migration name=MigrationName - Add new migration"
	@echo "  make remove-migration                 - Remove last migration"
	@echo "  make list-migrations                  - List all migrations"
	@echo "  make update-database                  - Update database to latest migration"
	@echo "  make generate-script                  - Generate SQL script"
	@echo "  make drop-database                    - Drop the database"
	@echo "  make help                             - Show this help message"

.PHONY: install-ef update-ef add-migration remove-migration list-migrations update-database generate-script drop-database help 
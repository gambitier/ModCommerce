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


### 3. JWT Configuration

The Identity Service uses RSA key pairs for JWT signing and validation. Follow these steps to generate and configure the JWT keys:

#### Generate RSA Key Pair

Run the following commands in your terminal to generate the RSA key pair:

```bash
# Generate private key
openssl genpkey -algorithm RSA -out private.pem -pkeyopt rsa_keygen_bits:2048

# Generate public key from private key
openssl rsa -pubout -in private.pem -out public.pem

# Convert private key to single line format
echo -n "$(awk 'NF {printf "%s",$0}' private.pem)" > private_single_line.pem

# Convert public key to single line format
echo -n "$(awk 'NF {printf "%s",$0}' public.pem)" > public_single_line.pem

# Clean up original files
rm -f private.pem public.pem
```

#### Configure Keys in appsettings.json

1. Open `appsettings.json` or your environment-specific settings file (e.g., `appsettings.Development.json`)
2. Locate the `Jwt.Keys` section
3. Configure the key settings as follows:
   ```json
   {
     "Jwt": {
       "Keys": [
         {
           "KeyId": "your-key-id",           // Unique identifier for this key pair
           "PrivateKeyPem": "",              // Paste contents from private_single_line.pem
           "PublicKeyPem": "",               // Paste contents from public_single_line.pem
           "IsActive": true                  // Set to true to make this the active key pair
         }
       ]
     }
   }
   ```
4. Once configured, you can delete generated files - `private_single_line.pem` & `public_single_line.pem`

#### Important Notes:

- Keep your private key secure and never commit it to source control
- The `KeyId` should be a unique identifier for the key pair (e.g., "key1", "2024-03-key", etc.)
- Multiple keys can be configured to support key rotation
- Only one key should have `"IsActive": true` at a time
- Store sensitive key information in environment-specific settings files (e.g., `appsettings.Development.json`) that are not committed to source control

#### Example Key Rotation:

```json
{
  "Jwt": {
    "Keys": [
      {
        "KeyId": "key-2024-03",
        "PrivateKeyPem": "-----BEGIN PRIVATE KEY-----...-----END PRIVATE KEY-----",
        "PublicKeyPem": "-----BEGIN PUBLIC KEY-----...-----END PUBLIC KEY-----",
        "IsActive": true
      },
      {
        "KeyId": "key-2024-02",
        "PrivateKeyPem": "-----BEGIN PRIVATE KEY-----...-----END PRIVATE KEY-----",
        "PublicKeyPem": "-----BEGIN PUBLIC KEY-----...-----END PUBLIC KEY-----",
        "IsActive": false
      }
    ]
  }
}
```

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

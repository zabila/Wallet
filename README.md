# 🏦 Wallet - Personal Finance Manager with Telegram Integration

[![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-13+-336791.svg)](https://www.postgresql.org/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-3.8+-FF6600.svg)](https://www.rabbitmq.com/)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED.svg)](https://www.docker.com/)
[![Telegram Bot](https://img.shields.io/badge/Telegram-Bot%20API-26A5E4.svg)](https://core.telegram.org/bots/api)

**Wallet** is a modern, microservices-based financial application built using Clean Architecture principles. The system provides efficient personal finance management through a convenient Telegram bot interface with transaction tracking, account management, and expense analytics capabilities.

## 📋 Table of Contents

- [🏗️ Architecture](#️-architecture)
- [✨ Key Features](#-key-features)
- [🛠️ Technology Stack](#️-technology-stack)
- [📦 Project Structure](#-project-structure)
- [🚀 Quick Start](#-quick-start)
- [⚙️ Configuration](#️-configuration)
- [🔧 API Documentation](#-api-documentation)
- [🧪 Testing](#-testing)
- [📱 Telegram Bot](#-telegram-bot)
- [🔐 Security](#-security)
- [📈 Monitoring](#-monitoring)
- [🤝 Contributing](#-contributing)

## 🏗️ Architecture

The project is built following **Clean Architecture** principles with a microservices approach:

```
┌─────────────────────────────────────────────────────────────┐
│                    TELEGRAM BOT UI                          │
├─────────────────────────────────────────────────────────────┤
│  API Gateway Layer                                          │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────────────────┐│
│  │ API.Identity│ │ API.Finance │ │    API.Telegram         ││
│  │             │ │             │ │                         ││
│  │ Auth & Users│ │ Transactions│ │   Bot Integration       ││
│  └─────────────┘ └─────────────┘ └─────────────────────────┘│
├─────────────────────────────────────────────────────────────┤
│                   Application Layer                         │
│  ┌─────────────────────────────────────────────────────────┐│
│  │           CQRS + MediatR + Domain Events               ││
│  └─────────────────────────────────────────────────────────┘│
├─────────────────────────────────────────────────────────────┤
│                    Domain Layer                             │
│  ┌───────────┐ ┌──────────────┐ ┌──────────────────────────┐│
│  │   Users   │ │  Accounts    │ │     Transactions         ││
│  └───────────┘ └──────────────┘ └──────────────────────────┘│
├─────────────────────────────────────────────────────────────┤
│                Infrastructure Layer                         │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────────────────┐│
│  │ PostgreSQL  │ │  RabbitMQ   │ │    External APIs        ││
│  │  Database   │ │   Message   │ │                         ││
│  │             │ │     Bus     │ │  Telegram Bot API       ││
│  └─────────────┘ └─────────────┘ └─────────────────────────┘│
└─────────────────────────────────────────────────────────────┘
```

### Microservices

- **API.Identity**: Authentication, authorization, and user management
- **API.Finance**: Account and transaction management
- **API.Telegram**: Telegram bot integration and message processing

## ✨ Key Features

### 💳 Financial Management
- ✅ **Account Management**: Create and manage multiple financial accounts
- ✅ **Transaction Tracking**: Detailed income and expense tracking with categorization
- ✅ **Multi-currency Support**: Support for different currencies
- ✅ **Real-time Balances**: Instant balance updates after transactions
- ✅ **Geolocation**: Link transactions to their location

### 👤 User Management
- 🔐 **JWT Authentication**: Secure authentication with JWT tokens
- 👥 **User Profiles**: Personalized profiles with localization
- 🔗 **Telegram Integration**: Link to Telegram profile

### 🤖 Telegram Bot
- 💬 **Interactive Interface**: Convenient management through Telegram
- 📊 **Instant Analytics**: Quick access to financial data
- 🔔 **Notifications**: Automatic notifications about important events
- 🌐 **Multi-language**: Support for different interface languages

## 🛠️ Technology Stack

### Backend
- **Framework**: .NET 9.0, ASP.NET Core
- **Language**: C# 12.0
- **Architecture**: Clean Architecture, CQRS, Domain-Driven Design
- **ORM**: Entity Framework Core 9.0
- **Database**: PostgreSQL 13+
- **Message Broker**: RabbitMQ 3.8+
- **Authentication**: JWT Bearer, ASP.NET Identity

### Infrastructure
- **Containerization**: Docker & Docker Compose
- **API Documentation**: Swagger/OpenAPI
- **Logging**: NLog
- **Patterns**: MediatR, Repository Pattern

### Testing
- **Unit Testing**: xUnit, Moq, FluentAssertions
- **Integration Testing**: TestContainers (PostgreSQL, RabbitMQ)
- **End-to-End Testing**: ASP.NET Core Testing Framework
- **Test Data**: Bogus, AutoFixture

### External APIs
- **Telegram Bot API**: Telegram.Bot 22.3.0
- **HTTP Client**: Flurl.Http

## 📦 Project Structure

```
📁 Wallet/
├── 📁 src/
│   ├── 📁 API.Identity/          # Identity microservice
│   ├── 📁 API.Finance/           # Finance microservice
│   ├── 📁 API.Telegram/          # Telegram bot service
│   ├── 📁 Application/           # Business logic (CQRS)
│   ├── 📁 Domain/                # Domain entities
│   ├── 📁 Infrastructure/        # Infrastructure layer
│   ├── 📁 MessageBus/            # Asynchronous messaging
│   └── 📁 SharedKernel/          # Shared components
├── 📁 tests/
│   ├── 📁 Unit/                  # Unit tests
│   ├── 📁 Integration/           # Integration tests
│   ├── 📁 EndToEnd/              # E2E tests
│   └── 📁 Shared/                # Test utilities
├── 📄 docker-compose.yml        # Docker configuration
├── 📄 Directory.Packages.props  # Centralized packages
└── 📄 Directory.Build.props     # MSBuild settings
```

## 🚀 Quick Start

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

### 1. Clone the repository

```bash
git clone https://github.com/your-username/wallet.git
cd wallet
```

### 2. Setup environment variables

Create a `.env` file in the project root:

```env
# Security
SECRET=your-super-secret-jwt-key-here-min-32-chars

# Telegram Bot
TELEGRAM_BOT_TOKEN=your-telegram-bot-token
WALLET_API_TOKEN=your-internal-api-token

# Database (optional, docker-compose is used by default)
DATABASE_CONNECTION_STRING=Host=localhost;Database=wallet;Username=postgres;Password=postgres
```

### 3. Run with Docker Compose

```bash
# Build and run all services
docker-compose up --build

# Run in background
docker-compose up -d --build
```

### 4. Verify the setup

- **Identity API**: http://localhost:5000/swagger
- **Finance API**: http://localhost:5001/swagger
- **PostgreSQL**: localhost:5432
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)

### 5. Local development setup

```bash
# Run only infrastructure
docker-compose up database rabbitmq -d

# Run Identity API
cd src/API.Identity
dotnet run

# Run Finance API (new terminal)
cd src/API.Finance
dotnet run

# Run Telegram Bot (new terminal)
cd src/API.Telegram
dotnet run
```

## ⚙️ Configuration

### Database Migrations

```bash
# Generate new migration
dotnet ef migrations add MigrationName -p src/Infrastructure -s src/API.Identity

# Apply migrations
dotnet ef database update -p src/Infrastructure -s src/API.Identity
```

### Environment Configuration

Each microservice supports different environments through `appsettings.{Environment}.json`:

- `appsettings.Development.json` - Local development
- `appsettings.Production.json` - Production environment

## 🔧 API Documentation

### Identity API Endpoints
```http
POST /api/auth/register     # User registration
POST /api/auth/login        # User login
GET  /api/users/profile     # User profile
PUT  /api/users/profile     # Update profile
```

### Finance API Endpoints
```http
GET    /api/accounts        # Get accounts
POST   /api/accounts        # Create account
GET    /api/transactions    # Get transactions
POST   /api/transactions    # Create transaction
PUT    /api/transactions/{id} # Update transaction
DELETE /api/transactions/{id} # Delete transaction
```

### Swagger UI
- **Identity API**: http://localhost:5000/swagger
- **Finance API**: http://localhost:5001/swagger

## 🧪 Testing

### Running tests

```bash
# All tests
dotnet test Wallet.sln
```

### Test types

- **Unit Tests**: Testing domain logic and business rules
- **Integration Tests**: Testing interaction with database and external services
- **End-to-End Tests**: Testing complete usage scenarios

## 📱 Telegram Bot

### Bot setup

1. Create a bot through [@BotFather](https://t.me/botfather)
2. Get the bot token
3. Add the token to the `.env` file
4. Start the `API.Telegram` service

## 🔐 Security

### Implemented security measures

- ✅ **JWT Authentication**: Secure authentication
- ✅ **Password Hashing**: Password hashing through ASP.NET Identity
- ✅ **CORS Policy**: Configured CORS policy
- ✅ **Data Protection**: Personal data protection
- ✅ **HTTPS Enforcement**: Enforced HTTPS usage
- ✅ **Input Validation**: Input data validation

### Production environment recommendations

- Use strong passwords for the database
- Configure SSL/TLS certificates
- Use Azure Key Vault or similar services for secrets
- Configure security monitoring and logging

## 📈 Monitoring

### Logging

The project uses **NLog** for structured logging:

- Logs are written to console and files
- Different logging levels (Debug, Info, Warning, Error)
- Structured logs for easy analysis

### Health Checks

```http
GET /health     # Service health check
```

## 🤝 Contributing

### How to contribute

1. Create a **feature branch**: `git checkout -b feature/amazing-feature`
2. **Commit** changes: `git commit -m 'Add amazing feature'`
3. **Push** to branch: `git push origin feature/amazing-feature`
4. Create a **Pull Request**

### Code standards

- Use EditorConfig settings
- Follow C# Coding Conventions
- Cover new code with tests
- Update documentation when necessary

### Bug reporting

Please use GitHub Issues for bug reports with detailed description:

- Steps to reproduce
- Expected behavior
- Actual behavior
- Version and environment

<div align="center">

**⭐ If the project was helpful, please give it a star on GitHub! ⭐**

Made with ❤️ and ☕ in Ukraine 🇺🇦

</div>

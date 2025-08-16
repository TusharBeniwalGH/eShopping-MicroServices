# E-Shop Microservices

A modern e-commerce platform built with .NET 8 microservices architecture, demonstrating best practices in distributed systems design and implementation.

## Overview

This project showcases a complete e-commerce solution with different business domains encapsulated in separate microservices:

* **Catalog** - Product management and inventory
* **Basket** - Shopping cart functionality  
* **Ordering** - Order processing and management
* **Discount** - Pricing and promotional services

Each microservice follows its own distinct architectural style with the database-per-service pattern, ensuring loose coupling and independent scalability.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## Quick Start

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd eshop-microservices
   ```

2. **Run with Docker Compose**
   ```bash
   cd src
   docker-compose up -d
   ```

3. **Access the services**
   - API Gateway: `http://localhost:6000`
   - Catalog API: `http://localhost:6001`
   - Basket API: `http://localhost:6002`
   - Ordering API: `http://localhost:6003`
   - Discount gRPC: `http://localhost:6004`

## Architecture

### Catalog Microservice
**Product management and inventory service**

* ASP.NET Core Minimal APIs with .NET 8 and C# 12
* **Vertical Slice Architecture** with feature folders
* CQRS implementation using MediatR library
* Validation pipeline with MediatR and FluentValidation
* Marten library for transactional document DB on PostgreSQL
* Carter for minimal API endpoint definition
* Cross-cutting concerns: logging, global exception handling, and health checks

### Basket Microservice
**Shopping cart and session management**

* ASP.NET 8 Web API following REST principles
* **Redis** as distributed cache for basket storage
* Implements Proxy, Decorator, and Cache-aside patterns
* Consumes Discount **gRPC service** for real-time price calculations
* Publishes BasketCheckout events using **MassTransit and RabbitMQ**
  
### Discount Microservice
**Pricing and promotional services**

* ASP.NET **gRPC Server** application
* High-performance **inter-service gRPC communication** with Basket service
* Protobuf message definitions for service contracts
* Entity Framework Core with SQLite provider
* Lightweight **SQLite database** with containerization support

### Ordering Microservice
**Order processing and management**

* **Domain-Driven Design (DDD)**, **CQRS**, and **Clean Architecture**
* MediatR, FluentValidation, and Mapster packages
* Consumes **RabbitMQ** BasketCheckout events via **MassTransit**
* **SQL Server** database with Entity Framework Core
* Auto-migration on application startup

### API Gateway
**Centralized entry point and routing**

* **YARP Reverse Proxy** implementing Gateway Routing Pattern
* Route, cluster, path, and destination configuration
* **KeyCloak** integration for authentication and authorization
* **Rate limiting** with FixedWindowLimiter

## Communication Patterns

### Synchronous Communication
* **gRPC** for high-performance inter-service calls
* Basket ↔ Discount service integration

### Asynchronous Communication
* **RabbitMQ** message broker with publish/subscribe pattern
* **MassTransit** abstraction layer
* BasketCheckout event flow: Basket → Ordering
* Shared **EventBus.Messages** library for event contracts

## Technology Stack

### Backend
- **.NET 8** - Latest LTS version with C# 12
- **ASP.NET Core** - Web APIs and minimal APIs
- **gRPC** - High-performance inter-service communication
- **MediatR** - CQRS and mediator pattern implementation
- **FluentValidation** - Input validation
- **Mapster** - Object mapping

### Databases
- **PostgreSQL** - Document storage with Marten
- **SQL Server** - Relational data for ordering
- **SQLite** - Lightweight storage for discounts
- **Redis** - Distributed caching

### Infrastructure
- **Docker & Docker Compose** - Containerization
- **RabbitMQ** - Message broker
- **MassTransit** - Messaging abstraction
- **YARP** - Reverse proxy and API gateway
- **KeyCloak** - Identity and access management

## Development

### Building the Solution
```bash
cd src
dotnet build eshop-microservices.sln
```

### Running Individual Services
```bash
# Catalog API
cd Services/Catalog/Catalog.API
dotnet run

# Basket API  
cd Services/Basket/Basket.API
dotnet run

# And so on...
```

### Database Migrations
```bash
# Ordering service
cd Services/Ordering/Ordering.Infrastructure
dotnet ef database update
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

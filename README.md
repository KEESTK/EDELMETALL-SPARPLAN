# 💰 Edelmetall-Sparplan Backend

![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet)
![EF Core](https://img.shields.io/badge/Entity%20Framework%20Core-9.0-green)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-blue)
![Docker](https://img.shields.io/badge/Docker-Ready-blue)
![License](https://img.shields.io/badge/License-MIT-lightgrey)

Dieses Projekt implementiert ein **Banking-Backend für Edelmetall-Sparpläne**.  
Es basiert auf **.NET 9**, **Entity Framework Core** und **PostgreSQL** und folgt einem klaren **DDD-Schichtenmodell**.

---

## 🚀 Features
- **Depots** als Container für Sparpläne  
- **Sparpläne** für Gold und Silber  
- **Transaktionen**: Einzahlung (Deposit), Gebühr (Fee), Auszahlung (Payout)  
- REST-API mit **Swagger/OpenAPI**  
- Persistenz mit **PostgreSQL** via EF Core  

---

## 🏗 Architekturübersicht

```
┌─────────────────────────────┐
│        Client / UI          │
│  (Swagger UI, Frontend,     │
│   Postman, etc.)            │
└───────────────▲─────────────┘
                │ HTTP/REST
┌───────────────┴─────────────┐
│       Sparplan.Api           │
│  ASP.NET Core WebAPI Layer   │
│  - Controller (REST Endpoints)│
│  - Swagger/OpenAPI           │
└───────────────▲─────────────┘
                │ DI / EF Core
┌───────────────┴─────────────┐
│  Sparplan.Infrastructure     │
│  - AppDbContext (EF Core)    │
│  - AppDbContextFactory       │
└───────────────▲─────────────┘
                │
┌───────────────┴─────────────┐
│       PostgreSQL DB          │
│  - Depots                    │
│  - Sparplaene                │
│  - Transactions              │
└───────────────▲─────────────┘
                │
┌───────────────┴─────────────┐
│     Sparplan.Domain          │
│  - SparplanClass             │
│  - Depot                     │
│  - Transaction + Type        │
│  - Metal + MetalType         │
└─────────────────────────────┘
```

---

## 📂 Projektstruktur

```
backend/
│── src/
│   ├── Sparplan.Api/              # ASP.NET Core API (Controller, Startup, Program)
│   ├── Sparplan.Infrastructure/   # EF Core DbContext, Migrations, Factory
│   ├── Sparplan.Domain/           # Domain-Modelle & Business-Logik
│   └── Sparplan.Application/      # (Option für Services, Business-UseCases)
│
│── Dockerfile                     # Docker Build für API
│── docker-compose.yml              # Startet Backend + PostgreSQL
│── Sparplan.sln                   # .NET Solution
```

---

## ⚙️ Setup & Start

### Voraussetzungen
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)

### Lokaler Start (mit Docker Compose)
```bash
# Backend + PostgreSQL starten
docker-compose up --build
```

- API erreichbar unter: [http://localhost:5001/swagger](http://localhost:5001/swagger)  
- Datenbank läuft auf: `localhost:5433`  
- frontend auf: `localhost:4200`

### Datenbankmigrationen ausführen
Falls neue Migrationen notwendig sind:
```bash
dotnet ef migrations add <Name> -p src/Sparplan.Infrastructure -s src/Sparplan.Api
dotnet ef database update -p src/Sparplan.Infrastructure -s src/Sparplan.Api
```

---

## 🔍 API Beispiele

### 📦 Depots
- **Depot erstellen**  
  `POST /api/depots`  
  → Erstellt ein leeres Depot.

- **Alle Depots abrufen**  
  `GET /api/depots`  

- **Depot nach Id abrufen**  
  `GET /api/depots/{id}`  

- **Sparplan hinzufügen**  
  `POST /api/depots/{id}/add-sparplan`  
  Body:  
  ```json
  {
    "metal": "Gold",
    "monthlyRate": 200
  }
  ```

---

### 📋 Sparpläne
- **Alle Sparpläne abrufen**  
  `GET /api/sparplaene`  

- **Sparplan nach Id abrufen**  
  `GET /api/sparplaene/{id}`  

⚠️ Hinweis: Sparpläne können **nur über ein Depot** angelegt werden.

---

### 💰 Transaktionen
- **Einzahlung buchen**  
  `POST /api/transactions/deposit`  
  ```json
  {
    "sparplanId": "uuid",
    "amountInBars": 1.5,
    "amountInCurrency": 1000
  }
  ```

- **Gebühr abbuchen**  
  `POST /api/transactions/fee`  
  ```json
  {
    "sparplanId": "uuid",
    "amountInBars": 0.5
  }
  ```

- **Auszahlung vornehmen**  
  `POST /api/transactions/payout`  
  ```json
  {
    "sparplanId": "uuid",
    "payoutValue": 2.0
  }
  ```

- **Alle Transaktionen eines Sparplans abrufen**  
  `GET /api/transactions/{sparplanId}`  

---

## 👤 Author
- Bewerberaufgabe *Softwareentwickler Banking 2025*  
- Technologie-Stack: **.NET 9 + EF Core + PostgreSQL + Docker**

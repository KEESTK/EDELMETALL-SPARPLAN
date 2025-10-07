# EDELMETALL-SPARPLAN
## Licensing and Usage
**This code is submitted exclusively for the purpose of job application assessment and technical evaluation for the Softwareentwickler C#/.NET Banking position at Software Entwicklung und IT-Dienstleistung GmBH (SPI).**

**All commercial rights, deployment rights, and intellectual property rights remain strictly reserved by the author (KEESTK).**

The recipient (the company) is granted a non-exclusive, non-transferable, and temporary license to review, compile, and execute this code solely to evaluate the applicant's skills. Any other use, including deployment, production use, distribution, or adaptation for commercial purposes, is strictly prohibited unless a formal employment contract is signed and executed.
---

# 🪙 Edelmetall-Sparplan
**Eine vollständige Web-Anwendung zur Verwaltung von Edelmetall-Sparplänen (Gold & Silber)**
Frontend (Angular + PrimeNG) • Backend (.NET 9 + EF Core + PostgreSQL) • Containerisiert mit Docker

---

## 💼 Ziel der Anwendung

Die Software ermöglicht es, **Depots** anzulegen, **Sparpläne** zu erstellen, **Transaktionen** (Einzahlungen, Gebühren, Schließungen) durchzuführen und **Simulationen** der Wertentwicklung auf Basis historischer Kursdaten darzustellen.
Sie deckt damit den gesamten Lebenszyklus eines Edelmetall-Sparplans ab – von der Eröffnung bis zur Auszahlung.

---

## ⚙️ Technologie-Stack

| Komponente            | Technologie                                              |
| --------------------- | -------------------------------------------------------- |
| **Frontend**          | Angular 20 • PrimeNG • Chart.js • TypeScript             |
| **Backend**           | .NET 9 • ASP.NET Core WebAPI • Entity Framework Core     |
| **Datenbank**         | PostgreSQL 16                                            |
| **Containerisierung** | Docker • Docker Compose                                  |
| **Architektur**       | Domain Driven Design (DDD) mit klarer Schichten-Trennung |

---

## 🚀 Haupt-Features

### Backend

* ✅ REST-API für Depots, Sparpläne und Transaktionen
* ✅ Transaktionstypen: Einzahlung, Gebühr, Sparplan schließen (Request + Confirm)
* ✅ DDD-Struktur mit Domain-Klassen (Depot, Sparplan, Transaction, MetalType)
* ✅ Swagger UI / OpenAPI-Dokumentation
* ✅ Persistenz über PostgreSQL & Entity Framework Core
* ✅ Docker-Support für API + DB

### Frontend

* ✅ Übersicht aller Depots mit Sparplänen
* ✅ Inline-Aktionen (Depot erstellen, Sparplan hinzufügen, Transaktion ausführen) — ohne modale Dialoge
* ✅ Visualisierung von Simulationsergebnissen mit Chart.js (Zoom & Pan)
* ✅ Dynamische API-Konfiguration über `assets/config.json` (`apiUrl`)
* ✅ Responsive Layout mit PrimeNG-Komponenten (Button, Table, Select usw.)

---

## 🏗 Architekturüberblick

```
┌──────────────────────────────────────────────────┐
│                  Frontend (Angular)              │
│  - Depot-Übersicht, Sparpläne, Transaktionen     │
│  - Simulation mit Chart.js + PrimeNG             │
│  - API-URL über assets/config.json               │
└───────────────────────▲──────────────────────────┘
                        │ REST/JSON
┌───────────────────────┴──────────────────────────┐
│              Backend (.NET 9 WebAPI)            │
│  - Controller (Depots, Sparplaene, Transactions) │
│  - Preisservice, Validierung, DTO-Mapping        │
│  - Swagger UI, EF Core                          │
└───────────────────────▲──────────────────────────┘
                        │ EF Core
┌───────────────────────┴──────────────────────────┐
│              PostgreSQL 16 Database             │
│  - Tables: Depots, Sparplaene, Transactions      │
│  - Beziehungen: 1 Depot → n Sparplaene → n Tx   │
└──────────────────────────────────────────────────┘
```

---

## 📦 Projektstruktur

```
edelmetall-sparplan/
│
├── backend/
│   ├── src/
│   │   ├── Sparplan.Api/              # ASP.NET Core WebAPI
│   │   ├── Sparplan.Infrastructure/   # EF Core Context + Migrations
│   │   ├── Sparplan.Domain/           # Domain Model (DDD)
│   │   └── Sparplan.Application/      # Businesslogik (optionales Layer)
│   │
│   ├── Dockerfile                     # Backend Dockerfile
│   ├── docker-compose.yml             # Backend + PostgreSQL
│   └── Sparplan.sln
│
└── frontend/
    ├── src/app/features/
    │   ├── depots/                    # Depotverwaltung
    │   ├── sparplaene/                # Sparplanverwaltung
    │   ├── transactions/              # Transaktionen (Deposit/Fee/Close)
    │   └── simulation/                # Simulation mit Chart.js
    │
    ├── assets/config.json             # API-Konfiguration
    ├── Dockerfile                     # Frontend Build + nginx Serve
    └── docker-compose.yml             # Frontend + Backend Integration
```

---

## ⚙️ Start (Docker-Compose)

### Voraussetzungen

* Docker & Docker Compose
* (optional) .NET 9 SDK und Node 20 für lokale Entwicklung

### Starten

```bash
# Im Projektverzeichnis
docker-compose up --build
```

Nach dem Start:

| Komponente                     | URL                                                            |
| ------------------------------ | -------------------------------------------------------------- |
| **Frontend (Angular + nginx)** | [http://localhost:8080](http://localhost:8080)                 |
| **Backend API + Swagger UI**   | [http://localhost:5001/swagger](http://localhost:5001/swagger) |
| **PostgreSQL DB**              | `localhost:5433`                                               |

---

## 🧩 Beispiel-Endpoints

### Depot

```bash
POST /api/depots
GET  /api/depots
GET  /api/depots/{id}
POST /api/depots/{id}/add-sparplan
```

### Sparplan

```bash
GET  /api/sparplaene
GET  /api/sparplaene/{id}
```

### Transaktionen

```bash
POST /api/transactions/deposit
POST /api/transactions/fee
POST /api/transactions/close/request
POST /api/transactions/close/confirm
GET  /api/transactions/{sparplanId}
```

---

## 📊 Simulation

Im Frontend kann der Benutzer:

1. Metall (Gold / Silber) wählen
2. Monatliche Rate und Zeitraum definieren
3. Simulation starten → Chart zeigt:

   * Einzahlungen (€)
   * Bestände (Barren)
   * Marktwert (€)
   * Gewinn/Verlust (€)

Zoom & Pan funktionieren per Mausrad und Touch-Gesten.

---

## 🔐 Hinweise

* API-URL wird über `frontend/assets/config.json` definiert → z. B. `"apiUrl": "/api"`.
* POST-Requests erfolgen ausschließlich über das Frontend, kein direkter Swagger-Zugriff erforderlich.
* Alle Container sind für **lokale Entwicklung** vorgesehen; kein Persistenzvolumen außerhalb von Docker notwendig.

---

## 👨‍💻 Author & Context

**Application Task: Software Developer Banking 2025**

| Attribute | Details |
| :--- | :--- |
| **Applicant** | Kees |
| **Total Implementation Time** | **Approx. 100 Hours** (across 7 days) |
| **Technology Stack** | .NET 9 • EF Core • PostgreSQL • Angular 20 • PrimeNG • Chart.js • Docker |

**Note on Time Commitment:** The depth of this implementation, including the full-stack architecture, comprehensive documentation, and production readiness, required this substantial time investment to fully demonstrate my skills for the role.

---


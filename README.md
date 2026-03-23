# What is this project?

The goal of this project is to build a live PnL calculator for users holding positions in equities and derivatives.

The system is designed to update PnL in real time, using Kafka to decouple services while maintaining high performance. Price updates should propagate through the system efficiently — updating in-memory PnL, recalculating user position values, pushing updates via WebSockets, and persisting realised PnL where needed.

This project is also a way for me to demonstrate my skillset in:
- Live PnL systems
- Valuation logic (especially derivatives)
- Event-driven architecture

---

# Backend

The backend is built entirely in **C#**, with an event-driven architecture powered by **Kafka**.

The idea is to keep services loosely coupled while ensuring fast and reliable data flow across the system.

---

# Frontend

The frontend uses:
- **Vite**
- **React**
- **TypeScript**

It provides a dashboard where users can manage positions and view their live PnL.

---

# The Plan

Since we can’t fully simulate real market data, we’ll fetch live prices from Yahoo Finance for equities.

## Core Workflow

- Users can search for assets and choose to buy/sell via a dashboard.
- When an order is placed, it is executed at the current price and stored.
- The position is persisted in a positions database.
- The dashboard displays live PnL across all user positions.

---

# Options Support

- Users can trade options alongside equities.
- Options are priced using Black-Scholes (with potential extensions like volatility smiles for more realistic pricing).
- Options can be:
  - Resold
  - Exercised

This adds a more realistic valuation layer to the system.

---

# System Design (Current Direction)

- Price updates are published to Kafka.
- A PnL consumer processes incoming price updates.

## Initial Setup

- On startup:
  - Build in-memory maps:
    - `user -> positions`
    - `ticker -> users`
  - Populate initial prices.

## Real-Time Updates

- As new prices arrive:
  - Update prices in memory *(long-term: introduce sharding for scalability)*.
  - Recalculate user PnL incrementally.
  - *(Potential improvement: partition PnL updates across Kafka for scalability.)*

## Data Propagation

- Updated PnL is published back to Kafka.
- The .NET API layer consumes this and pushes updates via WebSockets.
- WebSocket behavior:
  - Send an initial full snapshot
  - Then send incremental updates only
- Updates include:
  - Relevant price changes
  - Updated user PnL

---

# Next Steps

- Introduce a dedicated queue for order execution:
  - Orders executed at a defined price
  - Persisted both in memory and in the positions database
  - Include execution timestamps

- Extend valuation capabilities:
  - Support multiple option pricing models
  - Improve realism of derivatives pricing

- Ensure correct PnL tracking:
  - Unrealised PnL
  - Realised PnL

- Improve infrastructure:
  - Proper Kafka configuration (singleton injection, constants, etc.)
  - Scalability improvements (e.g. partitioning, sharding)
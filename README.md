# What is this project?

The goal of this project is to build a live PnL calculator for users holding positions in equities and derivatives.

The system is designed to update PnL in real time, using Kafka to decouple services while maintaining high performance. Price updates should propagate through the system efficiently. These updates should update in-memory (redis) PnL, recalculate user position values, push updates via WebSockets, and persist realised PnL where needed.

This project is also a way for me to demonstrate my skillset in:
- Live PnL systems
- Valuation logic
- Event-driven architecture

---

# Backend

The backend is built entirely in C#, with an event-driven architecture powered by Kafka, and an in-memory cache using Redis. 

The idea is to keep services loosely coupled while ensuring fast and reliable data flow across the system. Whilst I did not fully implement hexagonal architecture using ports and adapters, that will be part of the refactoring process. For the sake of getting a working prototype working, that structure has not been fully implemented.

---

# Frontend

The frontend uses:
- Vite
- React
- TypeScript

It provides a dashboard where users can manage positions and view their live PnL, and buy/sell positions.

---

# The Plan

Since we can’t fully simulate real market data, we’ll fetch live prices from Polygon for equities.

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
- The pricing engine then recalculates PnL per user using a reverse index.
- PnL updates are sent out using Kafka and ingested by the API hosting the websocket hub
- The updates are pushed to clients 

## Initial Setup

On startup:
- Build in-memory maps:
  - `user -> positions`
  - `ticker -> users`
- Populate initial prices.
- Make sure that Redis is hydrated.



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

- Improve infrastructure:
  - Scalability improvements (e.g. partitioning, sharding)


# Developer's Notes

- For options: model the IV dist based on last EOD and then recalc prices on surface 

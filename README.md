# 🎬 Movie Market - Cinema Ticketing System

A full-stack cinema ticketing platform built with **ASP.NET MVC**, allowing users to browse, book, and refund movie tickets, and enabling admins to manage the movie ecosystem effectively.

---

## 🔧 Tech Stack
- ASP.NET MVC
- Entity Framework Core
- SQL Server
- ASP.NET Identity (no JWT)
- LINQ, Middleware
- Bootstrap (for UI)

---

## 👥 User Roles

### Admin
- Login via Identity.
- Manage cinemas: create, edit (name, description, logo, address), delete (with all related movies).
- Manage movie categories.
- Add/edit/delete movies (StartDate, EndDate, category, cinema, image, description).
- Assign existing actors to movies.
- Track all orders and view total ticket revenue.
- Search movies by name.
- Edit own profile info.

### Customer
- Register/login via Identity.
- Browse all available movies with details, trailers, and assigned actors.
- Search/filter movies by name.
- Add available movies to cart (movies past EndDate are blocked via middleware).
- Secure checkout and booking.
- Partial or full refund available based on ticket count and movie expiration.
- Edit own profile info.

---

## 🧠 Core Features

- 🔐 Authentication and Role-based Authorization (ASP.NET Identity)
- 🧹 Middleware for Expired Movie Enforcement
- 🗃️ Full CRUD operations for Movies, Cinemas, Categories
- 🛒 Cart & Order Management
- 💸 Refund Logic (Partial & Full)
- 🔍 Dynamic Search Filter for Movies
- 🧑‍💼 Profile Management for Admins and Users

---

## 🖼️ Media Handling

- Movie and cinema images are uploaded and updated.
- On edit, previous images are automatically replaced and old files deleted.

---

## 📦 Database

- SQL Server database with normalized schema.
- Relationships:
  - One-to-Many: Cinema → Movies, Category → Movies
  - Many-to-Many: Movie ↔ Actors
  - One-to-Many: Order → OrderItems → Movie

---

## 🚀 Getting Started

1. Clone the repo
2. Update `appsettings.json` with your SQL Server connection string
3. Apply migrations and update database:

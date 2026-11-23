# Lecture Management System

A console-based application built in C# (.NET 10) for managing university lectures.

## Features

- Add lectures with auto-generated subject codes
- View, edit, and delete lectures by code
- Search lectures by subject name or lecturer
- List all stored lectures in a formatted table
- Data persisted to a local JSON file

## Lecture Types

| Type | Code Format | Extra Field |
|---|---|---|
| Programming | PRG-101, PRG-102, ... | Programming language |
| Database | DB-101, DB-102, ... | Database system |
| Networking | NET-101, NET-102, ... | Protocol |

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run

```bash
dotnet run
```

## Project Structure

| File | Purpose |
|---|---|
| `Lecture.cs` | Base lecture class |
| `Programming.cs` / `Database.cs` / `Networking.cs` | Lecture subtypes |
| `LectureManager.cs` | Business logic, code generation, LINQ queries |
| `FileHandler.cs` | JSON persistence |
| `Menu.cs` | Console UI |
| `Program.cs` | Entry point |

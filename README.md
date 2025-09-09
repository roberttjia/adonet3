# ADO.NET 3 Test Project

This is a test project demonstrating SQL Server stored procedures with ADO.NET.

## Features

- **Database Schema**: Orders, Customers, and Products tables
- **Stored Procedures**: CRUD operations and batch processing
- **C# Data Access**: ADO.NET implementation with SqlConnection
- **Unit Tests**: xUnit test cases for all stored procedures

## Database Objects

### Tables
- `Orders` - Order records with ExternalOrderId unique constraint
- `Customers` - Customer information
- `Products` - Product catalog

### Key Stored Procedures
- `proc_AddOrder` - Insert orders with duplicate ExternalOrderId checking
- `proc_UpdateOrder` - Update existing orders
- `proc_ArchiveCompletedOrders` - Batch archive with transactions
- `proc_ArchiveCompletedOrdersCursor` - Cursor-based archive with individual transactions

## Setup

1. Create database using `DbScripts/CreateDB.sql`
2. Update connection string in `appsettings.json`
3. Run tests to verify functionality

## Technology Stack

- .NET 8.0
- SQL Server
- ADO.NET (Microsoft.Data.SqlClient)
- xUnit for testing

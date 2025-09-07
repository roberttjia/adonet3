-- Create Database
CREATE DATABASE adonet3;
GO

USE adonet3;
GO

-- Create Customers Table
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    Address NVARCHAR(200),
    City NVARCHAR(50),
    State NVARCHAR(50),
    ZipCode NVARCHAR(10),
    CreatedDate DATETIME2 DEFAULT GETDATE()
);

-- Create Products Table
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(10,2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    Category NVARCHAR(50),
    CreatedDate DATETIME2 DEFAULT GETDATE()
);

-- Create Orders Table
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    UnitPrice DECIMAL(10,2) NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    OrderDate DATETIME2 DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Pending',
    
    -- Foreign Key Constraints
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    CONSTRAINT FK_Orders_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- Create Indexes for better performance
CREATE INDEX IX_Orders_CustomerID ON Orders(CustomerID);
CREATE INDEX IX_Orders_ProductID ON Orders(ProductID);
CREATE INDEX IX_Customers_Email ON Customers(Email);

-- Stored Procedure to Add Customer
CREATE PROCEDURE proc_AddCustomer
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20) = NULL,
    @Address NVARCHAR(200) = NULL,
    @City NVARCHAR(50) = NULL,
    @State NVARCHAR(50) = NULL,
    @ZipCode NVARCHAR(10) = NULL,
    @CustomerID INT OUTPUT
AS
BEGIN
    INSERT INTO Customers (FirstName, LastName, Email, Phone, Address, City, State, ZipCode)
    VALUES (@FirstName, @LastName, @Email, @Phone, @Address, @City, @State, @ZipCode);
    
    SET @CustomerID = SCOPE_IDENTITY();
END
GO
-- Stored Procedure to Get Customer by ID
CREATE PROCEDURE proc_GetCustomerById
    @CustomerID INT
AS
BEGIN
    SELECT CustomerID, FirstName, LastName, Email, Phone, Address, City, State, ZipCode, CreatedDate
    FROM Customers
    WHERE CustomerID = @CustomerID;
END
GO

-- Stored Procedure to Get Customers by Last Name
CREATE PROCEDURE proc_GetCustomersByLastName
    @LastName NVARCHAR(50)
AS
BEGIN
    SELECT CustomerID, FirstName, LastName, Email, Phone, Address, City, State, ZipCode, CreatedDate
    FROM Customers
    WHERE LastName LIKE @LastName + '%'
    ORDER BY LastName, FirstName;
END
GO

-- Stored Procedure to Update Customer
CREATE PROCEDURE proc_UpdateCustomer
    @CustomerID INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20) = NULL,
    @Address NVARCHAR(200) = NULL,
    @City NVARCHAR(50) = NULL,
    @State NVARCHAR(50) = NULL,
    @ZipCode NVARCHAR(10) = NULL
AS
BEGIN
    UPDATE Customers
    SET FirstName = @FirstName,
        LastName = @LastName,
        Email = @Email,
        Phone = @Phone,
        Address = @Address,
        City = @City,
        State = @State,
        ZipCode = @ZipCode
    WHERE CustomerID = @CustomerID;
    
    RETURN @@ROWCOUNT;
END
GO

-- Stored Procedure to Delete Customer by ID
CREATE PROCEDURE proc_DeleteCustomerById
    @CustomerID INT
AS
BEGIN
    DELETE FROM Customers
    WHERE CustomerID = @CustomerID;
    
    RETURN @@ROWCOUNT;
END
GO
-- Stored Procedure to Add Product
CREATE PROCEDURE proc_AddProduct
    @ProductName NVARCHAR(100),
    @Description NVARCHAR(500) = NULL,
    @Price DECIMAL(10,2),
    @StockQuantity INT = 0,
    @Category NVARCHAR(50) = NULL,
    @ProductID INT OUTPUT
AS
BEGIN
    INSERT INTO Products (ProductName, Description, Price, StockQuantity, Category)
    VALUES (@ProductName, @Description, @Price, @StockQuantity, @Category);
    
    SET @ProductID = SCOPE_IDENTITY();
END
GO

-- Stored Procedure to Get Product by ID
CREATE PROCEDURE proc_GetProductById
    @ProductID INT
AS
BEGIN
    SELECT ProductID, ProductName, Description, Price, StockQuantity, Category, CreatedDate
    FROM Products
    WHERE ProductID = @ProductID;
END
GO

-- Stored Procedure to Get Products by Category
CREATE PROCEDURE proc_GetProductsByCategory
    @Category NVARCHAR(50)
AS
BEGIN
    SELECT ProductID, ProductName, Description, Price, StockQuantity, Category, CreatedDate
    FROM Products
    WHERE Category LIKE @Category + '%'
    ORDER BY ProductName;
END
GO

-- Stored Procedure to Update Product
CREATE PROCEDURE proc_UpdateProduct
    @ProductID INT,
    @ProductName NVARCHAR(100),
    @Description NVARCHAR(500) = NULL,
    @Price DECIMAL(10,2),
    @StockQuantity INT = 0,
    @Category NVARCHAR(50) = NULL
AS
BEGIN
    UPDATE Products
    SET ProductName = @ProductName,
        Description = @Description,
        Price = @Price,
        StockQuantity = @StockQuantity,
        Category = @Category
    WHERE ProductID = @ProductID;
    
    RETURN @@ROWCOUNT;
END
GO

-- Stored Procedure to Delete Product by ID
CREATE PROCEDURE proc_DeleteProductById
    @ProductID INT
AS
BEGIN
    DELETE FROM Products
    WHERE ProductID = @ProductID;
    
    RETURN @@ROWCOUNT;
END
GO

USE [adonet3]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 9/8/2025 2:30:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](20) NULL,
	[Address] [nvarchar](200) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[ZipCode] [nvarchar](10) NULL,
	[CreatedDate] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 9/8/2025 2:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](10, 2) NOT NULL,
	[TotalAmount] [decimal](10, 2) NOT NULL,
	[OrderDate] [datetime2](7) NULL,
	[Status] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 9/8/2025 2:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[StockQuantity] [int] NOT NULL,
	[Category] [nvarchar](50) NULL,
	[CreatedDate] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ((1)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ('Pending') FOR [Status]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [StockQuantity]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Customers]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Products]
GO
/****** Object:  StoredProcedure [dbo].[proc_AddCustomer]    Script Date: 9/8/2025 2:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[proc_AddCustomer]
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
/****** Object:  StoredProcedure [dbo].[proc_AddProduct]    Script Date: 9/8/2025 2:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Stored Procedure to Add Product
CREATE PROCEDURE [dbo].[proc_AddProduct]
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
/****** Object:  StoredProcedure [dbo].[proc_DeleteCustomerById]    Script Date: 9/8/2025 2:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Stored Procedure to Delete Customer by ID
CREATE PROCEDURE [dbo].[proc_DeleteCustomerById]
    @CustomerID INT
AS
BEGIN
    DELETE FROM Customers
    WHERE CustomerID = @CustomerID;
    
    RETURN @@ROWCOUNT;
END
GO
/****** Object:  StoredProcedure [dbo].[proc_DeleteProductById]    Script Date: 9/8/2025 2:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Stored Procedure to Delete Product by ID
CREATE PROCEDURE [dbo].[proc_DeleteProductById]
    @ProductID INT
AS
BEGIN
    DELETE FROM Products
    WHERE ProductID = @ProductID;
    
    RETURN @@ROWCOUNT;
END
GO
/****** Object:  StoredProcedure [dbo].[proc_GetCustomerById]    Script Date: 9/8/2025 2:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Stored Procedure to Get Customer by ID
CREATE PROCEDURE [dbo].[proc_GetCustomerById]
    @CustomerID INT
AS
BEGIN
    SELECT CustomerID, FirstName, LastName, Email, Phone, Address, City, State, ZipCode, CreatedDate
    FROM Customers
    WHERE CustomerID = @CustomerID;
END
GO
/****** Object:  StoredProcedure [dbo].[proc_GetCustomersByLastName]    Script Date: 9/8/2025 2:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Stored Procedure to Get Customers by Last Name
CREATE PROCEDURE [dbo].[proc_GetCustomersByLastName]
    @LastName NVARCHAR(50)
AS
BEGIN
    SELECT CustomerID, FirstName, LastName, Email, Phone, Address, City, State, ZipCode, CreatedDate
    FROM Customers
    WHERE LastName LIKE @LastName + '%'
    ORDER BY LastName, FirstName;
END
GO
/****** Object:  StoredProcedure [dbo].[proc_GetProductById]    Script Date: 9/8/2025 2:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Stored Procedure to Get Product by ID
CREATE PROCEDURE [dbo].[proc_GetProductById]
    @ProductID INT
AS
BEGIN
    SELECT ProductID, ProductName, Description, Price, StockQuantity, Category, CreatedDate
    FROM Products
    WHERE ProductID = @ProductID;
END
GO
/****** Object:  StoredProcedure [dbo].[proc_GetProductsByCategory]    Script Date: 9/8/2025 2:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Stored Procedure to Get Products by Category
CREATE PROCEDURE [dbo].[proc_GetProductsByCategory]
    @Category NVARCHAR(50)
AS
BEGIN
    SELECT ProductID, ProductName, Description, Price, StockQuantity, Category, CreatedDate
    FROM Products
    WHERE Category LIKE @Category + '%'
    ORDER BY ProductName;
END
GO
/****** Object:  StoredProcedure [dbo].[proc_UpdateCustomer]    Script Date: 9/8/2025 2:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Stored Procedure to Update Customer
CREATE PROCEDURE [dbo].[proc_UpdateCustomer]
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
/****** Object:  StoredProcedure [dbo].[proc_UpdateProduct]    Script Date: 9/8/2025 2:30:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Stored Procedure to Update Product
CREATE PROCEDURE [dbo].[proc_UpdateProduct]
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

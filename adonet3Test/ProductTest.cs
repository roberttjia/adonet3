using Xunit;
using adonet3.Models;
using adonet3.DataAccess;
using System;

namespace adonet3Test
{
    public class ProductTest
    {
        [Fact]
        public void AddTenRandomProducts()
        {
            var dataAccess = new ProductDataAccess();
            var random = new Random();
            
            string[] productNames = { "Laptop", "Mouse", "Keyboard", "Monitor", "Headphones", "Tablet", "Phone", "Camera", "Speaker", "Printer" };
            string[] categories = { "Electronics", "Computers", "Audio", "Mobile", "Photography" };

            for (int i = 0; i < 10; i++)
            {
                var product = new Product
                {
                    ProductName = $"{productNames[random.Next(productNames.Length)]} {i + 1}",
                    Description = $"High quality {productNames[random.Next(productNames.Length)].ToLower()}",
                    Price = (decimal)(random.NextDouble() * 1000 + 50),
                    StockQuantity = random.Next(1, 100),
                    Category = categories[random.Next(categories.Length)]
                };

                var result = dataAccess.AddProduct(product);
                
                Assert.True(result.ProductID > 0, $"Product {i + 1} should have a valid ProductID");
                Console.WriteLine($"Added Product ID: {result.ProductID}, Name: {result.ProductName}, Price: ${result.Price:F2}");
            }
        }

        [Fact]
        public void GetProductById_ValidId_ReturnsProduct()
        {
            var dataAccess = new ProductDataAccess();
            
            // First add a product to get a valid ID
            var newProduct = new Product
            {
                ProductName = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                StockQuantity = 10,
                Category = "Test"
            };
            
            var addedProduct = dataAccess.AddProduct(newProduct);
            
            // Now retrieve the product
            var retrievedProduct = dataAccess.GetProductById(addedProduct.ProductID);
            
            Assert.NotNull(retrievedProduct);
            Assert.Equal(addedProduct.ProductID, retrievedProduct.ProductID);
            Assert.Equal("Test Product", retrievedProduct.ProductName);
            Assert.Equal(99.99m, retrievedProduct.Price);
        }

        [Fact]
        public void GetProductById_InvalidId_ReturnsNull()
        {
            var dataAccess = new ProductDataAccess();
            
            var result = dataAccess.GetProductById(99999);
            
            Assert.Null(result);
        }

        [Fact]
        public void GetProductsByCategory_ExistingCategory_ReturnsProducts()
        {
            var dataAccess = new ProductDataAccess();
            
            // Add products with same category
            var product1 = new Product { ProductName = "Product 1", Price = 50m, Category = "TestCategory" };
            var product2 = new Product { ProductName = "Product 2", Price = 75m, Category = "TestCategory" };
            
            dataAccess.AddProduct(product1);
            dataAccess.AddProduct(product2);
            
            var results = dataAccess.GetProductsByCategory("TestCategory");
            
            Assert.NotEmpty(results);
            Assert.True(results.Count >= 2);
            Assert.All(results, p => Assert.StartsWith("TestCategory", p.Category));
        }

        [Fact]
        public void GetProductsByCategory_NonExistentCategory_ReturnsEmptyList()
        {
            var dataAccess = new ProductDataAccess();
            
            var results = dataAccess.GetProductsByCategory("NonExistentCategory");
            
            Assert.Empty(results);
        }

        [Fact]
        public void UpdateProduct_ValidProduct_ReturnsRowsUpdated()
        {
            var dataAccess = new ProductDataAccess();
            
            // Add a product first
            var product = new Product
            {
                ProductName = "Original Product",
                Description = "Original Description",
                Price = 100m,
                StockQuantity = 5,
                Category = "Original"
            };
            
            var addedProduct = dataAccess.AddProduct(product);
            
            // Update the product
            addedProduct.ProductName = "Updated Product";
            addedProduct.Price = 150m;
            addedProduct.StockQuantity = 15;
            
            var rowsUpdated = dataAccess.UpdateProduct(addedProduct);
            
            Assert.Equal(1, rowsUpdated);
            
            // Verify the update
            var updatedProduct = dataAccess.GetProductById(addedProduct.ProductID);
            Assert.NotNull(updatedProduct);
            Assert.Equal("Updated Product", updatedProduct.ProductName);
            Assert.Equal(150m, updatedProduct.Price);
            Assert.Equal(15, updatedProduct.StockQuantity);
        }

        [Fact]
        public void UpdateProduct_NonExistentProduct_ReturnsZeroRows()
        {
            var dataAccess = new ProductDataAccess();
            
            var product = new Product
            {
                ProductID = 99999,
                ProductName = "Test Product",
                Price = 100m,
                StockQuantity = 10
            };
            
            var rowsUpdated = dataAccess.UpdateProduct(product);
            
            Assert.Equal(0, rowsUpdated);
        }

        [Fact]
        public void DeleteProductById_ValidId_ReturnsRowsDeleted()
        {
            var dataAccess = new ProductDataAccess();
            
            // Add a product first
            var product = new Product
            {
                ProductName = "Product To Delete",
                Price = 50m,
                StockQuantity = 1
            };
            
            var addedProduct = dataAccess.AddProduct(product);
            
            // Delete the product
            var rowsDeleted = dataAccess.DeleteProductById(addedProduct.ProductID);
            
            Assert.Equal(1, rowsDeleted);
            
            // Verify deletion
            var deletedProduct = dataAccess.GetProductById(addedProduct.ProductID);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public void DeleteProductById_NonExistentId_ReturnsZeroRows()
        {
            var dataAccess = new ProductDataAccess();
            
            var rowsDeleted = dataAccess.DeleteProductById(99999);
            
            Assert.Equal(0, rowsDeleted);
        }
    }
}

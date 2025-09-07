using Xunit;
using adonet3.Models;
using adonet3.DataAccess;
using System;

namespace adonet3Test
{
    public class CustomerTest
    {
        [Fact]
        public void AddTenRandomCustomers()
        {
            var dataAccess = new CustomerDataAccess();
            var random = new Random();
            
            string[] firstNames = { "John", "Jane", "Mike", "Sarah", "David", "Lisa", "Tom", "Anna", "Chris", "Emma" };
            string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
            string[] cities = { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia", "San Antonio", "San Diego", "Dallas", "San Jose" };
            string[] states = { "NY", "CA", "IL", "TX", "AZ", "PA", "TX", "CA", "TX", "CA" };

            for (int i = 0; i < 10; i++)
            {
                var customer = new Customer
                {
                    FirstName = firstNames[random.Next(firstNames.Length)],
                    LastName = lastNames[random.Next(lastNames.Length)],
                    Email = $"user{i + 1}@example.com",
                    Phone = $"555-{random.Next(1000, 9999)}",
                    Address = $"{random.Next(100, 9999)} Main St",
                    City = cities[random.Next(cities.Length)],
                    State = states[random.Next(states.Length)],
                    ZipCode = random.Next(10000, 99999).ToString()
                };

                var result = dataAccess.AddCustomer(customer);
                
                Assert.True(result.CustomerID > 0, $"Customer {i + 1} should have a valid CustomerID");
                Console.WriteLine($"Added Customer ID: {result.CustomerID}, Name: {result.FirstName} {result.LastName}");
            }
        }

        [Fact]
        public void GetCustomerById_ValidId_ReturnsCustomer()
        {
            var dataAccess = new CustomerDataAccess();
            
            // First add a customer to get a valid ID
            var newCustomer = new Customer
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Phone = "555-1234"
            };
            
            var addedCustomer = dataAccess.AddCustomer(newCustomer);
            
            // Now retrieve the customer
            var retrievedCustomer = dataAccess.GetCustomerById(addedCustomer.CustomerID);
            
            Assert.NotNull(retrievedCustomer);
            Assert.Equal(addedCustomer.CustomerID, retrievedCustomer.CustomerID);
            Assert.Equal("Test", retrievedCustomer.FirstName);
            Assert.Equal("User", retrievedCustomer.LastName);
        }

        [Fact]
        public void GetCustomerById_InvalidId_ReturnsNull()
        {
            var dataAccess = new CustomerDataAccess();
            
            var result = dataAccess.GetCustomerById(99999);
            
            Assert.Null(result);
        }

        [Fact]
        public void GetCustomersByLastName_ExistingLastName_ReturnsCustomers()
        {
            var dataAccess = new CustomerDataAccess();
            
            // Add customers with same last name
            var customer1 = new Customer { FirstName = "John", LastName = "TestName", Email = "john@test.com" };
            var customer2 = new Customer { FirstName = "Jane", LastName = "TestName", Email = "jane@test.com" };
            
            dataAccess.AddCustomer(customer1);
            dataAccess.AddCustomer(customer2);
            
            var results = dataAccess.GetCustomersByLastName("TestName");
            
            Assert.NotEmpty(results);
            Assert.True(results.Count >= 2);
            Assert.All(results, c => Assert.StartsWith("TestName", c.LastName));
        }

        [Fact]
        public void GetCustomersByLastName_NonExistentLastName_ReturnsEmptyList()
        {
            var dataAccess = new CustomerDataAccess();
            
            var results = dataAccess.GetCustomersByLastName("NonExistentName");
            
            Assert.Empty(results);
        }

        [Fact]
        public void UpdateCustomer_ValidCustomer_ReturnsRowsUpdated()
        {
            var dataAccess = new CustomerDataAccess();
            
            // Add a customer first
            var customer = new Customer
            {
                FirstName = "Original",
                LastName = "Name",
                Email = "original@test.com"
            };
            
            var addedCustomer = dataAccess.AddCustomer(customer);
            
            // Update the customer
            addedCustomer.FirstName = "Updated";
            addedCustomer.Email = "updated@test.com";
            
            var rowsUpdated = dataAccess.UpdateCustomer(addedCustomer);
            
            Assert.Equal(1, rowsUpdated);
            
            // Verify the update
            var updatedCustomer = dataAccess.GetCustomerById(addedCustomer.CustomerID);
            Assert.NotNull(updatedCustomer);
            Assert.Equal("Updated", updatedCustomer.FirstName);
            Assert.Equal("updated@test.com", updatedCustomer.Email);
        }

        [Fact]
        public void UpdateCustomer_NonExistentCustomer_ReturnsZeroRows()
        {
            var dataAccess = new CustomerDataAccess();
            
            var customer = new Customer
            {
                CustomerID = 99999,
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com"
            };
            
            var rowsUpdated = dataAccess.UpdateCustomer(customer);
            
            Assert.Equal(0, rowsUpdated);
        }

        [Fact]
        public void DeleteCustomerById_ValidId_ReturnsRowsDeleted()
        {
            var dataAccess = new CustomerDataAccess();
            
            // Add a customer first
            var customer = new Customer
            {
                FirstName = "ToDelete",
                LastName = "User",
                Email = "delete@test.com"
            };
            
            var addedCustomer = dataAccess.AddCustomer(customer);
            
            // Delete the customer
            var rowsDeleted = dataAccess.DeleteCustomerById(addedCustomer.CustomerID);
            
            Assert.Equal(1, rowsDeleted);
            
            // Verify deletion
            var deletedCustomer = dataAccess.GetCustomerById(addedCustomer.CustomerID);
            Assert.Null(deletedCustomer);
        }

        [Fact]
        public void DeleteCustomerById_NonExistentId_ReturnsZeroRows()
        {
            var dataAccess = new CustomerDataAccess();
            
            var rowsDeleted = dataAccess.DeleteCustomerById(99999);
            
            Assert.Equal(0, rowsDeleted);
        }
    }
}

using Xunit;
using adonet3.Models;
using adonet3.DataAccess;
using System;

namespace adonet3Test
{
    public class OrderTest
    {
        [Fact]
        public void AddOrder_Success()
        {
            var dataAccess = new OrderDataAccess();
            var order = new Order
            {
                CustomerID = 1,
                ProductID = 1,
                Quantity = 2,
                UnitPrice = 10.50m,
                TotalAmount = 21.00m,
                Status = "Pending",
                ExternalOrderId = "EXT-" + Guid.NewGuid().ToString()
            };

            int orderId = dataAccess.AddOrder(order);
            
            Assert.True(orderId > 0);
        }

        [Fact]
        public void AddOrder_DuplicateExternalOrderId_ReturnsNegativeNine()
        {
            var dataAccess = new OrderDataAccess();
            string externalId = "DUPLICATE-" + Guid.NewGuid().ToString();
            
            var order1 = new Order
            {
                CustomerID = 1,
                ProductID = 1,
                Quantity = 1,
                UnitPrice = 5.00m,
                TotalAmount = 5.00m,
                ExternalOrderId = externalId
            };

            var order2 = new Order
            {
                CustomerID = 2,
                ProductID = 2,
                Quantity = 1,
                UnitPrice = 10.00m,
                TotalAmount = 10.00m,
                ExternalOrderId = externalId
            };

            int firstOrderId = dataAccess.AddOrder(order1);
            int secondOrderId = dataAccess.AddOrder(order2);
            
            Assert.True(firstOrderId > 0);
            Assert.Equal(-9, secondOrderId);
        }

        [Fact]
        public void ArchiveCompletedOrders_Success()
        {
            var dataAccess = new OrderDataAccess();
            
            // Add orders with COMPLETED status
            var completedOrder1 = new Order
            {
                CustomerID = 1,
                ProductID = 1,
                Quantity = 1,
                UnitPrice = 15.00m,
                TotalAmount = 15.00m,
                Status = "COMPLETED",
                ExternalOrderId = "COMP-" + Guid.NewGuid().ToString()
            };

            var completedOrder2 = new Order
            {
                CustomerID = 2,
                ProductID = 2,
                Quantity = 2,
                UnitPrice = 20.00m,
                TotalAmount = 40.00m,
                Status = "COMPLETED",
                ExternalOrderId = "COMP-" + Guid.NewGuid().ToString()
            };

            int orderId1 = dataAccess.AddOrder(completedOrder1);
            int orderId2 = dataAccess.AddOrder(completedOrder2);
            
            var (errorCode, archivedOrderIds) = dataAccess.ArchiveCompletedOrders();
            
            Assert.Equal(0, errorCode);
            Assert.Contains(orderId1, archivedOrderIds);
            Assert.Contains(orderId2, archivedOrderIds);
        }

        [Fact]
        public void AddOrder_WithoutExternalOrderId_Success()
        {
            var dataAccess = new OrderDataAccess();
            var order = new Order
            {
                CustomerID = 1,
                ProductID = 1,
                Quantity = 3,
                UnitPrice = 7.50m,
                TotalAmount = 22.50m,
                Status = "Pending"
            };

            int orderId = dataAccess.AddOrder(order);
            
            Assert.True(orderId > 0);
        }

        [Fact]
        public void UpdateOrder_Success()
        {
            var dataAccess = new OrderDataAccess();
            
            // Add an order first
            var order = new Order
            {
                CustomerID = 1,
                ProductID = 1,
                Quantity = 1,
                UnitPrice = 10.00m,
                TotalAmount = 10.00m,
                Status = "Pending",
                ExternalOrderId = "UPDATE-" + Guid.NewGuid().ToString()
            };

            int orderId = dataAccess.AddOrder(order);
            
            // Update the order
            order.OrderID = orderId;
            order.Quantity = 5;
            order.UnitPrice = 12.00m;
            order.TotalAmount = 60.00m;
            order.Status = "COMPLETED";

            int rowsAffected = dataAccess.UpdateOrder(order);
            
            Assert.Equal(1, rowsAffected);
        }

        [Fact]
        public void UpdateOrder_NonExistentOrder_ReturnsZero()
        {
            var dataAccess = new OrderDataAccess();
            var order = new Order
            {
                OrderID = 999999,
                CustomerID = 1,
                ProductID = 1,
                Quantity = 1,
                UnitPrice = 10.00m,
                TotalAmount = 10.00m,
                Status = "Pending"
            };

            int rowsAffected = dataAccess.UpdateOrder(order);
            
            Assert.Equal(0, rowsAffected);
        }
    }
}

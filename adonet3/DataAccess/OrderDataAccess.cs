using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using adonet3.Models;
using adonet3.Utils;

namespace adonet3.DataAccess
{
    public class OrderDataAccess
    {
        private readonly string _connectionString;

        public OrderDataAccess()
        {
            _connectionString = ConnectionHelper.GetConnectionString();
        }

        public int AddOrder(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("proc_AddOrder", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                    command.Parameters.AddWithValue("@ProductID", order.ProductID);
                    command.Parameters.AddWithValue("@Quantity", order.Quantity);
                    command.Parameters.AddWithValue("@UnitPrice", order.UnitPrice);
                    command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    command.Parameters.AddWithValue("@Status", (object)order.Status ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ExternalOrderId", (object)order.ExternalOrderId ?? DBNull.Value);
                    
                    var orderIdParam = new SqlParameter("@OrderID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(orderIdParam);
                    
                    command.ExecuteNonQuery();
                    return (int)orderIdParam.Value;
                }
            }
        }

        public (int errorCode, List<int> archivedOrderIds) ArchiveCompletedOrders()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("proc_ArchiveCompletedOrders", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    var errorCodeParam = new SqlParameter("@ErrorCode", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(errorCodeParam);
                    
                    var archivedOrderIds = new List<int>();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            archivedOrderIds.Add(reader.GetInt32(0));
                        }
                    }
                    
                    int errorCode = (int)errorCodeParam.Value;
                    return (errorCode, archivedOrderIds);
                }
            }
        }

        public int UpdateOrder(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("proc_UpdateOrder", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@OrderID", order.OrderID);
                    command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                    command.Parameters.AddWithValue("@ProductID", order.ProductID);
                    command.Parameters.AddWithValue("@Quantity", order.Quantity);
                    command.Parameters.AddWithValue("@UnitPrice", order.UnitPrice);
                    command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    command.Parameters.AddWithValue("@Status", (object)order.Status ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ExternalOrderId", (object)order.ExternalOrderId ?? DBNull.Value);
                    
                    return command.ExecuteNonQuery();
                }
            }
        }

        public List<ArchivedOrder> ArchiveCompletedOrdersCursor()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("proc_ArchiveCompletedOrdersCursor", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    var adapter = new SqlDataAdapter(command);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    
                    var results = new List<ArchivedOrder>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        results.Add(new ArchivedOrder
                        {
                            OrderId = (int)row["OrderId"],
                            Archived = (bool)row["Archived"]
                        });
                    }
                    
                    return results;
                }
            }
        }
    }
}

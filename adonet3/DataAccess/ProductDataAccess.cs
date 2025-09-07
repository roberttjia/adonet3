using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using adonet3.Models;
using adonet3.Utils;

namespace adonet3.DataAccess
{
    public class ProductDataAccess
    {
        private readonly string _connectionString;

        public ProductDataAccess()
        {
            _connectionString = ConnectionHelper.GetConnectionString();
        }

        public Product AddProduct(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                using (var command = new SqlCommand("proc_AddProduct", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                    command.Parameters.AddWithValue("@Category", product.Category ?? (object)DBNull.Value);
                    
                    var productIdParam = new SqlParameter("@ProductID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(productIdParam);
                    
                    command.ExecuteNonQuery();
                    
                    product.ProductID = (int)productIdParam.Value;
                }
            }
            
            return product;
        }

        public Product? GetProductById(int productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                using (var command = new SqlCommand("proc_GetProductById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", productId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                ProductID = reader.GetInt32(0),
                                ProductName = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                StockQuantity = reader.GetInt32(4),
                                Category = reader.IsDBNull(5) ? null : reader.GetString(5),
                                CreatedDate = reader.GetDateTime(6)
                            };
                        }
                    }
                }
            }
            
            return null;
        }

        public List<Product> GetProductsByCategory(string category)
        {
            var products = new List<Product>();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("proc_GetProductsByCategory", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Category", category);
                    
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new System.Data.DataTable();
                        adapter.Fill(dataTable);
                        
                        foreach (System.Data.DataRow row in dataTable.Rows)
                        {
                            products.Add(new Product
                            {
                                ProductID = (int)row[0],
                                ProductName = (string)row[1],
                                Description = row[2] == DBNull.Value ? null : (string)row[2],
                                Price = (decimal)row[3],
                                StockQuantity = (int)row[4],
                                Category = row[5] == DBNull.Value ? null : (string)row[5],
                                CreatedDate = (DateTime)row[6]
                            });
                        }
                    }
                }
            }
            
            return products;
        }

        public int UpdateProduct(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                using (var command = new SqlCommand("proc_UpdateProduct", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                    command.Parameters.AddWithValue("@Category", product.Category ?? (object)DBNull.Value);
                    
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int DeleteProductById(int productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                using (var command = new SqlCommand("proc_DeleteProductById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", productId);
                    
                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using adonet3.Models;
using adonet3.Utils;

namespace adonet3.DataAccess
{
    public class CustomerDataAccess
    {
        private readonly string _connectionString;

        public CustomerDataAccess()
        {
            _connectionString = ConnectionHelper.GetConnectionString();
        }

        public Customer AddCustomer(Customer customer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                using (var command = new SqlCommand("proc_AddCustomer", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@Phone", customer.Phone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@City", customer.City ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@State", customer.State ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ZipCode", customer.ZipCode ?? (object)DBNull.Value);
                    
                    var customerIdParam = new SqlParameter("@CustomerID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(customerIdParam);
                    
                    command.ExecuteNonQuery();
                    
                    customer.CustomerID = (int)customerIdParam.Value;
                }
            }
            
            return customer;
        }

        public Customer? GetCustomerById(int customerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                using (var command = new SqlCommand("proc_GetCustomerById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", customerId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Customer
                            {
                                CustomerID = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                Phone = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Address = reader.IsDBNull(5) ? null : reader.GetString(5),
                                City = reader.IsDBNull(6) ? null : reader.GetString(6),
                                State = reader.IsDBNull(7) ? null : reader.GetString(7),
                                ZipCode = reader.IsDBNull(8) ? null : reader.GetString(8),
                                CreatedDate = reader.GetDateTime(9)
                            };
                        }
                    }
                }
            }
            
            return null;
        }

        public List<Customer> GetCustomersByLastName(string lastName)
        {
            var customers = new List<Customer>();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("proc_GetCustomersByLastName", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LastName", lastName);
                    
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new System.Data.DataTable();
                        adapter.Fill(dataTable);
                        
                        foreach (System.Data.DataRow row in dataTable.Rows)
                        {
                            customers.Add(new Customer
                            {
                                CustomerID = (int)row[0],
                                FirstName = (string)row[1],
                                LastName = (string)row[2],
                                Email = (string)row[3],
                                Phone = row[4] == DBNull.Value ? null : (string)row[4],
                                Address = row[5] == DBNull.Value ? null : (string)row[5],
                                City = row[6] == DBNull.Value ? null : (string)row[6],
                                State = row[7] == DBNull.Value ? null : (string)row[7],
                                ZipCode = row[8] == DBNull.Value ? null : (string)row[8],
                                CreatedDate = (DateTime)row[9]
                            });
                        }
                    }
                }
            }
            
            return customers;
        }

        public int UpdateCustomer(Customer customer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                using (var command = new SqlCommand("proc_UpdateCustomer", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@Phone", customer.Phone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@City", customer.City ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@State", customer.State ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ZipCode", customer.ZipCode ?? (object)DBNull.Value);
                    
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int DeleteCustomerById(int customerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                using (var command = new SqlCommand("proc_DeleteCustomerById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", customerId);
                    
                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}

using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Lab6.Models;

namespace Lab6.Controllers;

[ApiController]
    [Route("[controller]")]
    public class WarehouseController : ControllerBase
    {

        private readonly string connectionString = "Server=localhost;Database=APBD;User Id=sa;Password=asdP929klks";

        [HttpPost]
        public IActionResult AddProductToWarehouse([FromBody] ProductWarehouseRequest request)
        {
            if (request.Amount <= 0)
            {
                return BadRequest("Amount has to be greater than zero");
            }

            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;
            
            connection.Open();
            var transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            try 
            {
                command.CommandText = @"SELECT * FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount";
                command.Parameters.AddWithValue("@IdProduct", request.ProductId);
                command.Parameters.AddWithValue("@Amount", request.Amount);
                command.Parameters.AddWithValue("@IdWarehouse", request.WarehouseId);
                command.Parameters.AddWithValue("@FulfilledAt", DateTime.Now);
                

                Order order = new Order();
                int k = 0;
                using (var foundOrder = command.ExecuteReader())
                {
                    while (foundOrder.Read())
                    {
                        order = new Order
                        {
                            IdOrder = (int)foundOrder["IdOrder"],
                            Amount = (int)foundOrder["Amount"],
                            CreatedAt = (DateTime)foundOrder["CreatedAt"],
                            //FulfilledAt = (DateTime)foundOrder["FulfilledAt"],
                            IdProduct = (int)foundOrder["IdProduct"]
                        };
                        k++;
                    }

                    if (k == 0)
                    {
                        return NotFound($"No product found with Id {request.ProductId}");
                    }
                }

                if (order.CreatedAt >= request.CreatedAt)
                {
                    return BadRequest("Datetime of request is lower than datetime of order");
                }
                
                command.CommandText = $"SELECT * FROM Product_Warehouse WHERE IdOrder = {order.IdOrder}";
                using (var foundProductWarehouse = command.ExecuteReader())
                {

                    ProductWarehouse productWarehouse = new ProductWarehouse();
                    k = 0;
                    while (foundProductWarehouse.Read())
                    {
                        k++;
                    }

                    if (k > 0)
                    {
                        return BadRequest("Order is already completed");
                    }
                }

                var product = new Product();
                command.CommandText = @"SELECT * FROM Product WHERE IdProduct = @IdProduct";
                using (var foundProduct = command.ExecuteReader())
                {
                    k = 0;

                    while (foundProduct.Read())
                    {
                        product = new Product
                        {
                            IdProduct = (int)foundProduct["IdProduct"],
                            Price = Convert.ToDouble(foundProduct["Price"])
                        };
                        k++;
                    }

                    if (k == 0)
                    {
                        return NotFound($"There is no Product with {request.ProductId}");
                    }
                }


                command.CommandText = $@"UPDATE [Order] SET FulfilledAt = @FulfilledAt WHERE IdOrder = {order.IdOrder}";
                
                command.ExecuteNonQuery();

                var list = new List<int>();
                list.Add(0);

                command.CommandText = "SELECT * FROM Product_Warehouse";
                using (var foundAllProductWarehouse = command.ExecuteReader())
                {
                    while (foundAllProductWarehouse.Read())
                    {
                        list.Add((int)foundAllProductWarehouse["IdProductWarehouse"]);
                    }
                }

                var newId = list.Max() + 1;

                var price = product.Price * request.Amount;
                
                

                command.CommandText = @$"INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)
                                        VALUES ({newId}, @IdProduct, @IdOrder, @Amount, @Price, @FulfilledAt)";
                command.Parameters.AddWithValue("@IdOrder", order.IdOrder);
                command.Parameters.AddWithValue("@Price", price);
                command.ExecuteNonQuery();
                
                transaction.Commit();
                return Ok(newId);
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, $"An error occured: {ex.Message}");
            }
        }
    }
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using apbd6.Models;



namespace apbd6.Controllers;
[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public WarehouseController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    
    [HttpPost]
        public async Task<IActionResult> AddProductToWarehouse(ProductWarehouse request)
        {
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
                await connection.OpenAsync();

                //if product exists
                using var checkProductCommand = new SqlCommand("SELECT COUNT(*) FROM Product WHERE IdProduct = @IdProduct", connection);
                checkProductCommand.Parameters.AddWithValue("@IdProduct", request.IdProduct);
                var productCount = (int)await checkProductCommand.ExecuteScalarAsync();
                if (productCount == 0)
                {
                    return BadRequest("Product with provided IdProduct does not exist.");
                }

                //if warehouse exists
                using var checkWarehouseCommand = new SqlCommand("SELECT COUNT(*) FROM Warehouse WHERE IdWarehouse = @IdWarehouse", connection);
                checkWarehouseCommand.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
                var warehouseCount = (int)await checkWarehouseCommand.ExecuteScalarAsync();
                if (warehouseCount == 0)
                {
                    return BadRequest("Warehouse with provided IdWarehouse does not exist.");
                }

                //if there is a corresponding order
                var orderId = request.IdOrder;
                if (orderId == 0)
                {
                    return BadRequest("Invalid IdOrder.");
                }

                //if the order has been fulfilled and if there is no record in Product_Warehouse
                using var checkFulfilledCommand = new SqlCommand("SELECT IdProductWarehouse FROM Product_Warehouse WHERE IdOrder = @IdOrder", connection);
                checkFulfilledCommand.Parameters.AddWithValue("@IdOrder", orderId);
                var productWarehouseId = await checkFulfilledCommand.ExecuteScalarAsync();
                if (productWarehouseId != null)
                {
                    return BadRequest("Order has already been fulfilled.");
                }

                //Update order FulfilledAt column
                using var updateOrderCommand = new SqlCommand("UPDATE [Order] SET FulfilledAt = GETDATE() WHERE IdOrder = @IdOrder", connection);
                updateOrderCommand.Parameters.AddWithValue("@IdOrder", orderId);
                await updateOrderCommand.ExecuteNonQueryAsync();

                //Get price from Product table
                using var getPriceCommand = new SqlCommand("SELECT Price FROM Product WHERE IdProduct = @IdProduct", connection);
                getPriceCommand.Parameters.AddWithValue("@IdProduct", request.IdProduct);
                var price = Convert.ToDecimal(await getPriceCommand.ExecuteScalarAsync());

                //Insert into Product_Warehouse
                using var insertCommand = new SqlCommand("INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, GETDATE()); SELECT SCOPE_IDENTITY();", connection);
                insertCommand.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
                insertCommand.Parameters.AddWithValue("@IdProduct", request.IdProduct);
                insertCommand.Parameters.AddWithValue("@IdOrder", orderId);
                insertCommand.Parameters.AddWithValue("@Amount", request.Amount);
                insertCommand.Parameters.AddWithValue("@Price", price * request.Amount);
                var insertedId = await insertCommand.ExecuteScalarAsync();

                return Ok(insertedId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
    
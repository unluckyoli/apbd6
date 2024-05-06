using System.Data;
using apbd6.Models;
using Microsoft.Data.SqlClient;

namespace apbd6.Repositiories;

public class Warehouses2Repository : IWarehouses2Repository
{
    private IConfiguration _configuration;
    public Warehouses2Repository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<int> InsertProductWarehouse(ProductWarehouse productWarehouse)
    {
        await using (var connectString = new SqlConnection(_configuration.GetConnectionString("Default")))
        {

            using var command = new SqlCommand("AddProductToWarehouse", connectString)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
            command.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
            command.Parameters.AddWithValue("@Amount", productWarehouse.Amount);
            command.Parameters.AddWithValue("@CreatedAt", productWarehouse.CreatedAt);

            await connectString.OpenAsync();

            var primaryKey = Convert.ToInt32(await command.ExecuteScalarAsync());
            return primaryKey;
        }
    }
}
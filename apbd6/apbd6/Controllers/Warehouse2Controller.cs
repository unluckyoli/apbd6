using apbd6.Models;
using apbd6.Repositiories;
using Microsoft.AspNetCore.Mvc;

namespace apbd6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Warehouse2Controller : ControllerBase
{
    private IWarehouses2Repository _warehouses2Repository;
    public Warehouse2Controller(IWarehouses2Repository warehouses2Repository)
    {
        _warehouses2Repository = warehouses2Repository;
    }

    //insert
    [HttpPost]
    public async Task<int> AddProductToWarehouses(ProductWarehouse productWarehouse)
    {
        return await _warehouses2Repository.InsertProductWarehouse(productWarehouse);
    }
}
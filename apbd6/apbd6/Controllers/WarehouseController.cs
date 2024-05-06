using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using apbd6.Models;
using apbd6.Repositiories;


namespace apbd6.Controllers;
[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{

    private readonly IWarehousesRepository _warehousesRepository;
    
    //private readonly IConfiguration _configuration;
    /*public WarehouseController(IConfiguration configuration)
    {
        _configuration = configuration;
    }*/
    
    public WarehouseController(IWarehousesRepository warehousesRepository)
    {
        _warehousesRepository = warehousesRepository;
    }
    
    //add productWarhouse
    [HttpPost]
    public async Task<IActionResult> AddProductWarehouse(ProductWarehouse productWarehouse)
    {
        if (await _warehousesRepository.ProductNotExist(productWarehouse.IdProduct) 
            || await _warehousesRepository.WarehouseNotExist(productWarehouse.IdWarehouse))
        {
            return NotFound("produkt albo warehouse nie istnieja");
        }

        int idOrder = await _warehousesRepository.OrderNotExist(productWarehouse);
        if (idOrder == -1)
        {
            return NotFound("nie ma takiego orderu");
        }
        
        

        
        
        await _warehousesRepository.UpdateFulfilledAt(idOrder);

        var primaryKey = await _warehousesRepository.InsertProductWarehouse(productWarehouse, idOrder);
            
        return Ok(primaryKey);
    }
    }
    
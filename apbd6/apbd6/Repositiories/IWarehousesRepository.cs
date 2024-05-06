using apbd6.Models;

namespace apbd6.Repositiories;

public interface IWarehousesRepository
{
    Task<bool> ProductNotExist(int id);
    
    Task<bool> WarehouseNotExist(int id);
    
    Task<int> InsertProductWarehouse(ProductWarehouse productWarehouse, int idOrder);
    
    Task<int> OrderNotExist(ProductWarehouse warehouse);
    
    Task UpdateFulfilledAt(int idOrder);
}
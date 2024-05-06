using apbd6.Models;

namespace apbd6.Repositiories;

public interface IWarehouses2Repository
{
    Task<int> InsertProductWarehouse(ProductWarehouse productWarehouse);
}
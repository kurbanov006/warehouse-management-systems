public interface ISupplierService
{
    bool Create(Supplier supplier);
    bool Update(Supplier supplier);
    bool Delete(int id);
    IEnumerable<Supplier> GetAll();
    Supplier GetById(int id);
    IEnumerable<Supplier> GettingAListOfSuppliersWhoHaveProductsWithACertainQuantityInStock(int minProductQuantity);
}
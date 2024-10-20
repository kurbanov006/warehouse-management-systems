public interface IProductService
{
    bool Create(Product product);
    bool Update(Product product);
    bool Delete(int id);
    IEnumerable<Product> GetAll();
    Product GetById(int id);
    IEnumerable<Product> ReceivingProductsFilteredByCategoryAndSortedByPrice();
    IEnumerable<Product> RetrievingItemsWhoseQuantityIsLessThanASpecifiedValue(int maxQuantity);
    IEnumerable<Product> ReceiveAllItemsThatHaveBeenOrderedMoreThan5Times();
}
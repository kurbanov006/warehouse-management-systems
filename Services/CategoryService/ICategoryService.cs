public interface ICategoryService
{
    bool Create(Category category);
    bool Update(Category category);
    bool Delete(int id);
    IEnumerable<Category> GetAll();
    Category GetById(int id);
    IEnumerable<object> GettingACategoryWithTheNumberOfProductsInEachCategory();
}
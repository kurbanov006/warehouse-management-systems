public interface IOrderService
{
    bool Create(Order order);
    bool Update(Order order);
    bool Delete(int id);
    IEnumerable<Order> GetAll();
    Order GetById(int id);
    IEnumerable<Order> GetAllOrdersForASpecificSupplierFilteredByStatus(int supplierid, string status);
    IEnumerable<Order> GetOrderInformationByDateRange(DateTime startDate, DateTime endDate);
}
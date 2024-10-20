
using System.Xml.Linq;

public class OrderService : IOrderService
{
    private readonly string path;
    public OrderService(IConfiguration configuration)
    {
        path = configuration.GetSection(Const.PathData).Value!;

        if (!File.Exists(path) || new FileInfo(path).Length == 0)
        {
            XDocument xDocument = new XDocument();
            xDocument.Declaration = new XDeclaration("1.0", "utf-8", "true");
            XElement xElement = new XElement(Const.Sourse, new XElement(Const.Orders));
            xDocument.Add(xElement);
            xDocument.Save(path);
        }
    }
    public bool Create(Order order)
    {
        XDocument doc = XDocument.Load(path);
        int maxId = 0;

        doc.Element(Const.Sourse)!.Add(new XElement(Const.Orders));

        if (doc.Element(Const.Sourse)!.Element(Const.Orders)!.HasElements)
        {
            maxId = (int)doc.Element(Const.Sourse)!.Element(Const.Orders)!.Elements(Const.Order).Select(x => x.Element(Const.Id)).LastOrDefault()!;
        }

        XElement xElement = new XElement(Const.Order,
        new XElement(Const.Id, maxId + 1),
        new XElement(Const.ProductId, order.ProductId),
        new XElement(Const.Quantity, order.Quantity),
        new XElement(Const.OrderDate, order.OrderDate),
        new XElement(Const.SupplierId, order.SupplierId),
        new XElement(Const.Status, order.Status)
        );

        doc.Element(Const.Sourse)!.Element(Const.Orders)!.Add(xElement);
        doc.Save(path);
        return true;
    }

    public bool Delete(int id)
    {
        XDocument doc = XDocument.Load(path);

        XElement? order = doc.Element(Const.Sourse)!.Element(Const.Orders)!.Elements(Const.Order).FirstOrDefault(x => (int)x.Element(Const.Id)! == id);
        if (order == null)
            return false;

        order.Remove();
        doc.Save(path);
        return true;
    }

    public IEnumerable<Order> GetAll()
    {
        XDocument doc = XDocument.Load(path);

        List<Order> orders = doc.Element(Const.Sourse)!.Element(Const.Orders)!.Elements(Const.Order)
        .Select(x => new Order
        {
            Id = (int)x.Element(Const.Id)!,
            ProductId = (int)x.Element(Const.ProductId)!,
            Quantity = (int)x.Element(Const.Quantity)!,
            OrderDate = DateTime.TryParse(Const.OrderDate, out var orderDate) ? orderDate : DateTime.MinValue,
            SupplierId = (int)x.Element(Const.SupplierId)!,
            Status = (string)x.Element(Const.Status)!
        }).ToList();

        if (orders == null)
            return null!;

        return orders;
    }

    public IEnumerable<Order> GetAllOrdersForASpecificSupplierFilteredByStatus(int supplierid, string status)
    {
        XDocument doc = XDocument.Load(path);

        var res = from o in doc.Descendants(Const.Order)
                  where int.Parse(o.Element(Const.SupplierId)!.Value) == supplierid
                  && o.Element(Const.Status)!.Value == status
                  select new Order
                  {
                      Id = (int)o.Element(Const.Id)!,
                      SupplierId = (int)o.Element(Const.SupplierId)!,
                      Status = (string)o.Element(Const.Status)!,
                      Quantity = (int)o.Element(Const.Quantity)!
                  };

        if (res == null)
            return null!;

        return res;
    }

    public Order GetById(int id)
    {
        XDocument doc = XDocument.Load(path);

        XElement? order = doc.Element(Const.Sourse)!.Element(Const.Orders)!.Elements(Const.Order).FirstOrDefault(x => (int)x.Element(Const.Id)! == id);
        if (order == null)
            return null!;

        return new Order
        {
            Id = (int)order.Element(Const.Id)!,
            ProductId = (int)order.Element(Const.ProductId)!,
            Quantity = (int)order.Element(Const.Quantity)!,
            OrderDate = DateTime.TryParse(Const.OrderDate, out var orderDate) ? orderDate : DateTime.MinValue,
            SupplierId = (int)order.Element(Const.SupplierId)!,
            Status = (string)order.Element(Const.Status)!
        };
    }

    public IEnumerable<Order> GetOrderInformationByDateRange(DateTime startDate, DateTime endDate)
    {
        XDocument doc = XDocument.Load(path);

        var res = from o in doc.Descendants(Const.Order)
                  let date = (DateTime)o.Element(Const.OrderDate)!
                  where date > startDate && date < endDate
                  select o;

        if (res == null)
            return null!;

        return res.Select(x => new Order
        {
            Id = (int)x.Element(Const.Id)!,
            OrderDate = (DateTime)x.Element(Const.OrderDate)!
        });
    }

    public bool Update(Order order)
    {
        XDocument doc = XDocument.Load(path);

        XElement? updateOrder = doc.Element(Const.Sourse)!.Element(Const.Orders)!
        .Elements(Const.Order).FirstOrDefault(x => (int)x.Element(Const.Id)! == order.Id);

        if (updateOrder == null)
            return false;

        updateOrder.SetElementValue(Const.ProductId, order.ProductId);
        updateOrder.SetElementValue(Const.Quantity, order.Quantity);
        updateOrder.SetElementValue(Const.OrderDate, order.OrderDate);
        updateOrder.SetElementValue(Const.SupplierId, order.SupplierId);
        updateOrder.SetElementValue(Const.Status, order.Status);

        doc.Save(path);
        return true;
    }
}


file class Const
{
    public const string PathData = "PathData";
    public const string Sourse = "sourse";
    public const string Orders = "orders";
    public const string Order = "order";
    public const string Id = "id";
    public const string ProductId = "productid";
    public const string Quantity = "quantity";
    public const string OrderDate = "orderdate";
    public const string SupplierId = "supplierid";
    public const string Status = "status";

}
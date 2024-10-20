
using System.Xml.Linq;

public class SupplierService : ISupplierService
{
    private readonly string path;
    public SupplierService(IConfiguration configuration)
    {
        path = configuration.GetSection(Const.PathData).Value!;
        if (!File.Exists(path) || new FileInfo(path).Length == 0)
        {
            XDocument xDocument = new XDocument();
            xDocument.Declaration = new XDeclaration("1.0", "utf-8", "true");
            XElement xElement = new XElement(Const.Sourse, new XElement(Const.Suppliers));
            xDocument.Add(xElement);
            xDocument.Save(path);
        }
    }
    public bool Create(Supplier supplier)
    {
        XDocument doc = XDocument.Load(path);
        int maxId = 0;

        doc.Element(Const.Sourse)!.Add(new XElement(Const.Suppliers));

        if (doc.Element(Const.Sourse)!.Element(Const.Suppliers)!.HasElements)
        {
            maxId = (int)doc.Element(Const.Sourse)!.Element(Const.Suppliers)!
            .Elements(Const.Supplier)
            .Select(x => x.Element(Const.Id))
            .LastOrDefault()!;
        }

        XElement xElement = new XElement(Const.Supplier,
        new XElement(Const.Id, maxId + 1),
        new XElement(Const.Name, supplier.Name),
        new XElement(Const.ContactPerson, supplier.ContactPerson),
        new XElement(Const.Email, supplier.Email),
        new XElement(Const.Phone, supplier.Phone)
        );

        doc.Element(Const.Sourse)!.Element(Const.Suppliers)!.Add(xElement);
        doc.Save(path);
        return true;
    }

    public bool Delete(int id)
    {
        XDocument doc = XDocument.Load(path);

        XElement? supplier = doc.Element(Const.Sourse)!.Element(Const.Suppliers)!
        .Elements(Const.Supplier).FirstOrDefault(x => (int)x.Element(Const.Id)! == id);
        if (supplier == null)
            return false;

        supplier.Remove();
        doc.Save(path);
        return true;
    }

    public IEnumerable<Supplier> GetAll()
    {
        XDocument doc = XDocument.Load(path);

        IEnumerable<Supplier> suppliers = doc.Element(Const.Sourse)!.Element(Const.Suppliers)!
        .Elements(Const.Supplier)!
        .Select(x => new Supplier
        {
            Id = (int)x.Element(Const.Id)!,
            Name = (string)x.Element(Const.Name)!,
            ContactPerson = (string)x.Element(Const.ContactPerson)!,
            Email = (string)x.Element(Const.Email)!,
            Phone = (string)x.Element(Const.Phone)!
        }).ToList();

        if (suppliers == null)
            return null!;

        return suppliers;
    }

    public Supplier GetById(int id)
    {
        XDocument doc = XDocument.Load(path);

        XElement supplier = doc.Element(Const.Sourse)!
        .Element(Const.Suppliers)!.
        Elements(Const.Supplier).FirstOrDefault
        (x => (int)x.Element(Const.Id)! == id)!;

        if (supplier == null)
            return null!;

        return new Supplier
        {
            Id = (int)supplier.Element(Const.Id)!,
            Name = (string)supplier.Element(Const.Name)!,
            ContactPerson = (string)supplier.Element(Const.ContactPerson)!,
            Email = (string)supplier.Element(Const.Email)!,
            Phone = (string)supplier.Element(Const.Phone)!
        };
    }

    public bool Update(Supplier supplier)
    {
        XDocument doc = XDocument.Load(path);

        XElement updateSupplier = doc.Element(Const.Sourse)!
        .Element(Const.Suppliers)!.Elements(Const.Supplier).FirstOrDefault
        (x => (int)x.Element(Const.Id)! == supplier.Id)!;

        if (updateSupplier == null)
            return false;

        updateSupplier.SetElementValue(Const.Name, supplier.Name);
        updateSupplier.SetElementValue(Const.ContactPerson, supplier.ContactPerson);
        updateSupplier.SetElementValue(Const.Email, supplier.Email);
        updateSupplier.SetElementValue(Const.Phone, supplier.Phone);
        doc.Save(path);
        return true;
    }

    public IEnumerable<Supplier> GettingAListOfSuppliersWhoHaveProductsWithACertainQuantityInStock(int minProductQuantity)
    {
        XDocument doc = XDocument.Load(path);

        var res = from s in doc.Descendants(Const.Supplier)
                  join o in doc.Descendants(Const.Order) on s.Element(Const.Id) equals o.Element(Const.SupplierId)
                  join p in doc.Descendants(Const.Product) on o.Element(Const.ProductId) equals p.Element(Const.ProductId)
                  where int.Parse(p.Element(Const.Quantity)!.Value) == minProductQuantity
                  select s;

        return res.Select(x => new Supplier
        {
            Id = (int)x.Element(Const.Id)!,
            Name = (string)x.Element(Const.Name)!
        });
    }
}


file class Const
{
    public const string PathData = "PathData";
    public const string Sourse = "sourse";
    public const string Suppliers = "suppliers";
    public const string Supplier = "supplier";
    public const string Id = "id";
    public const string Name = "name";
    public const string ContactPerson = "contactperson";
    public const string Email = "email";
    public const string Phone = "phone";
    public const string Order = "order";
    public const string Product = "product";
    public const string ProductId = "productid";
    public const string Quantity = "quantity";
    public const string SupplierId = "supplierid";
}
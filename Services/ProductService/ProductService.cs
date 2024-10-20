
using System.Linq.Expressions;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

public class ProductService : IProductService
{
    private readonly string path;
    public ProductService(IConfiguration configuration)
    {
        path = configuration.GetSection(Const.PathData).Value!;
        if (!File.Exists(path) || new FileInfo(path).Length == 0)
        {
            XDocument xDocument = new XDocument();
            xDocument.Declaration = new XDeclaration("1.0", "utf-8", "true");
            XElement xElement = new XElement(Const.Sourse, new XElement(Const.Products));
            xDocument.Add(xElement);
            xDocument.Save(path);
        }
    }
    public bool Create(Product product)
    {
        XDocument doc = XDocument.Load(path);
        int maxId = 0;

        doc.Element(Const.Sourse)!.Add(new XElement(Const.Products));

        if (doc.Element(Const.Sourse)!.Element(Const.Products)!.HasElements)
        {
            maxId = (int)doc.Element(Const.Sourse)!.Element(Const.Products)!
            .Elements(Const.Product).Select(x => x.Element(Const.Id)).LastOrDefault()!;
        }

        XElement xElement = new XElement(Const.Product,
        new XElement(Const.Id, maxId + 1),
        new XElement(Const.Name, product.Name),
        new XElement(Const.Description, product.Description),
        new XElement(Const.Quantity, product.Quantity),
        new XElement(Const.Price, product.Price),
        new XElement(Const.CategoryId, product.CategoryId)
        );

        doc.Element(Const.Sourse)!.Element(Const.Products)!.Add(xElement);
        doc.Save(path);
        return true;
    }

    public bool Delete(int id)
    {
        XDocument doc = XDocument.Load(path);

        XElement? product = doc.Element(Const.Sourse)!.Element(Const.Products)!
        .Elements(Const.Product).FirstOrDefault(x => (int)x.Element(Const.Id)! == id);
        if (product == null)
            return false;

        product.Remove();
        doc.Save(path);
        return true;
    }

    public IEnumerable<Product> GetAll()
    {
        XDocument doc = XDocument.Load(path);

        List<Product> products = doc.Element(Const.Sourse)!.Element(Const.Products)!.Elements(Const.Product)
        .Select(x => new Product
        {
            Id = (int)x.Element(Const.Id)!,
            Name = (string)x.Element(Const.Name)!,
            Description = (string)x.Element(Const.Description)!,
            Quantity = (int)x.Element(Const.Quantity)!,
            Price = (decimal)x.Element(Const.Price)!,
            CategoryId = (int)x.Element(Const.CategoryId)!
        }).ToList();

        if (products == null)
            return null!;

        return products;
    }

    public Product GetById(int id)
    {
        XDocument doc = XDocument.Load(path);

        XElement? product = doc.Element(Const.Sourse)!.Element(Const.Products)!
        .Elements(Const.Product).FirstOrDefault(x => (int)x.Element(Const.Id)! == id);
        if (product == null)
            return null!;

        return new Product
        {
            Id = (int)product.Element(Const.Id)!,
            Name = (string)product.Element(Const.Name)!,
            Description = (string)product.Element(Const.Description)!,
            Quantity = (int)product.Element(Const.Quantity)!,
            Price = (decimal)product.Element(Const.Price)!,
            CategoryId = (int)product.Element(Const.CategoryId)!
        };
    }

    public bool Update(Product product)
    {
        XDocument doc = XDocument.Load(path);

        XElement? updateProduct = doc.Element(Const.Sourse)!.Element(Const.Products)!.Elements(Const.Product)
        .FirstOrDefault(x => (int)x.Element(Const.Id)! == product.Id);
        if (updateProduct == null)
            return false;

        updateProduct.SetElementValue(Const.Name, product.Name);
        updateProduct.SetElementValue(Const.Description, product.Description);
        updateProduct.SetElementValue(Const.Quantity, product.Quantity);
        updateProduct.SetElementValue(Const.Price, product.Price);
        updateProduct.SetElementValue(Const.CategoryId, product.CategoryId);
        doc.Save(path);
        return true;
    }

    public IEnumerable<Product> ReceivingProductsFilteredByCategoryAndSortedByPrice()
    {
        XDocument doc = XDocument.Load(path);

        IEnumerable<XElement> res = from p in doc.Descendants(Const.Product)
                                    let c = p.Element(Const.CategoryId)!.Value
                                    let pr = decimal.Parse(p.Element(Const.Price)!.Value)
                                    orderby c, pr
                                    select p;

        if (res == null)
            return null!;

        return res.Select(x => new Product
        {
            Id = (int)x.Element(Const.Id)!,
            Name = (string)x.Element(Const.Name)!,
            Price = (decimal)x.Element(Const.Price)!
        });
    }

    public IEnumerable<Product> RetrievingItemsWhoseQuantityIsLessThanASpecifiedValue(int maxQuantity)
    {
        XDocument doc = XDocument.Load(path);

        var res = from p in doc.Descendants(Const.Product)
                  where int.Parse(p.Element(Const.Quantity)!.Value) < maxQuantity
                  select p;
        if (res == null)
            return null!;

        return res.Select(x => new Product
        {
            Id = (int)x.Element(Const.Id)!,
            Name = (string)x.Element(Const.Name)!,
            Quantity = (int)x.Element(Const.Quantity)!
        });
    }

    public IEnumerable<Product> ReceiveAllItemsThatHaveBeenOrderedMoreThan5Times()
    {
        XDocument doc = XDocument.Load(path);

        var res = from o in doc.Descendants(Const.Order)
                  join p in doc.Descendants(Const.Product) on (int)o.Element(Const.ProductId)! equals (int)p.Element(Const.Id)!
                  group p by (int)p.Element(Const.Id)! into g
                  where g.Count() > 5
                  select new Product
                  {
                      Id = g.Key,
                      Name = (string)g.First().Element(Const.Name)!
                  };

        if (res == null)
            return null!;
        return res;
    }
}


file class Const
{
    public const string PathData = "PathData";
    public const string Sourse = "sourse";
    public const string Products = "products";
    public const string Product = "product";
    public const string ProductId = "productid";
    public const string Order = "order";
    public const string Id = "id";
    public const string Name = "name";
    public const string Description = "description";
    public const string Quantity = "quantity";
    public const string Price = "price";
    public const string CategoryId = "categoryid";
}
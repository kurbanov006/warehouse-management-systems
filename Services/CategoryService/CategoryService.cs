
using System.Xml.Linq;

public class CategoryService : ICategoryService
{
    private readonly string path;
    public CategoryService(IConfiguration configuration)
    {
        path = configuration.GetSection(Const.PathData).Value!;
        if (!File.Exists(path) || new FileInfo(path).Length == 0)
        {
            XDocument xDocument = new XDocument();
            xDocument.Declaration = new XDeclaration("1.0", "utf-8", "true");
            XElement xElement = new XElement(Const.Sourse, new XElement(Const.Categories));
            xDocument.Add(xElement);
            xDocument.Save(path);
        }
    }
    public bool Create(Category category)
    {
        XDocument doc = XDocument.Load(path);
        int maxId = 0;

        doc.Element(Const.Sourse)!.Add(new XElement(Const.Categories));

        if (doc.Element(Const.Sourse)!.Element(Const.Categories)!.HasElements)
        {
            maxId = (int)doc.Element(Const.Sourse)!.Element(Const.Categories)!.Elements(Const.Category).Select(x => x.Element(Const.Id)).LastOrDefault()!;
        }

        XElement xElement = new XElement(Const.Category,
        new XElement(Const.Id, maxId + 1),
        new XElement(Const.Name, category.Name),
        new XElement(Const.Description, category.Description)
        );

        doc.Element(Const.Sourse)!.Element(Const.Categories)!.Add(xElement);
        doc.Save(path);
        return true;
    }

    public bool Delete(int id)
    {
        XDocument doc = XDocument.Load(path);

        XElement? category = doc.Element(Const.Sourse)!.Element(Const.Categories)!.Elements(Const.Category).FirstOrDefault(x => (int)x.Element(Const.Id)! == id);
        if (category == null)
            return false;

        category.Remove();
        doc.Save(path);
        return true;
    }

    public IEnumerable<Category> GetAll()
    {
        XDocument doc = XDocument.Load(path);

        List<Category> categories = doc.Element(Const.Sourse)!.Element(Const.Categories)!.Elements(Const.Category)
        .Select(x => new Category
        {
            Id = (int)x.Element(Const.Id)!,
            Name = (string)x.Element(Const.Name)!,
            Description = (string)x.Element(Const.Description)!
        }).ToList();

        if (categories == null)
            return null!;

        return categories;
    }

    public Category GetById(int id)
    {
        XDocument doc = XDocument.Load(path);

        XElement? category = doc.Element(Const.Sourse)!.Element(Const.Categories)!.Elements(Const.Category).FirstOrDefault(x => (int)x.Element(Const.Id)! == id);
        if (category == null)
            return null!;

        return new Category
        {
            Id = (int)category.Element(Const.Id)!,
            Name = (string)category.Element(Const.Name)!,
            Description = (string)category.Element(Const.Description)!,
        };
    }

    public bool Update(Category category)
    {
        XDocument doc = XDocument.Load(path);

        XElement? updateCategory = doc.Element(Const.Sourse)!.Element(Const.Categories)!.Elements(Const.Category)
        .FirstOrDefault(x => (int)x.Element(Const.Id)! == category.Id);
        if (updateCategory == null)
            return false;

        updateCategory.SetElementValue(Const.Name, category.Name);
        updateCategory.SetElementValue(Const.Description, category.Description);
        doc.Save(path);
        return true;
    }

    public IEnumerable<object> GettingACategoryWithTheNumberOfProductsInEachCategory()
    {
        XDocument doc = XDocument.Load(path);

        var res = from c in doc.Descendants(Const.Category)
                  join p in doc.Descendants(Const.Product) on (int)c.Element(Const.Id)! equals (int)p.Element(Const.CategoryId)!
                  group c.Element(Const.Name) by (string)p.Element(Const.Name)! into g
                  select new
                  {
                      Name = g.Key,
                      Count = g.Count()
                  };
        return res.Select(x => new
        {
            Names = x.Name,
            Counts = x.Count
        });
    }
}


file class Const
{
    public const string PathData = "PathData";
    public const string Sourse = "sourse";
    public const string Categories = "categories";
    public const string Category = "category";
    public const string CategoryId = "categoryid";
    public const string Product = "product";
    public const string Id = "id";
    public const string Name = "name";
    public const string Description = "description";
}
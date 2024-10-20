


using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/product/")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IResult Create([FromBody] Product product)
    {
        if (product == null)
            return Results.BadRequest("Не получилось добавить");

        bool res = _productService.Create(product);
        if (res == false)
            return Results.BadRequest("Не получилось добавить");

        return Results.Ok("Успешно добавлено");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IResult Delete([FromRoute] int id)
    {
        bool res = _productService.Delete(id);
        if (res == false)
            return Results.BadRequest("Не удалось удалить!");

        return Results.Ok("Успешно удалено!");
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IResult Update([FromBody] Product product)
    {
        if (product == null)
            return Results.BadRequest("Не получилось обновить!");

        bool res = _productService.Update(product);
        if (res == false)
            return Results.BadRequest("Не получилось обновить");

        return Results.Ok("Успешно обновлено!");
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GetById([FromRoute] int id)
    {
        Product product = _productService.GetById(id);
        if (product == null)
            return Results.NotFound("Не удалось найти!");

        return Results.Ok(product);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GetAll()
    {
        IEnumerable<Product> products = _productService.GetAll();
        if (products == null)
            return Results.NotFound("Не удалось найти!");

        return Results.Ok(products);
    }

    // Query 1
    [HttpGet("sort")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult ReceivingProductsFilteredByCategoryAndSortedByPrice()
    {
        IEnumerable<Product> products = _productService.ReceivingProductsFilteredByCategoryAndSortedByPrice();
        if (products == null)
            return Results.NotFound("Не удалось найти!");

        return Results.Ok(products);
    }

    //Query 3
    [HttpGet("maxQuantity={maxQuantity}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult RetrievingItemsWhoseQuantityIsLessThanASpecifiedValue(int maxQuantity)
    {
        IEnumerable<Product> products = _productService.RetrievingItemsWhoseQuantityIsLessThanASpecifiedValue(maxQuantity);
        if (products == null)
            return Results.NotFound("Не удалось найти!");

        return Results.Ok(products);
    }

    [HttpGet("minOrders=5")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult ReceiveAllItemsThatHaveBeenOrderedMoreThan5Times()
    {
        IEnumerable<Product> products = _productService.ReceiveAllItemsThatHaveBeenOrderedMoreThan5Times();
        if (products == null)
            return Results.NotFound("Не удалось найти!");

        return Results.Ok(products);
    }


}
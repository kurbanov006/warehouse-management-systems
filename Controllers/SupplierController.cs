

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/supplier/")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;
    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IResult Create([FromBody] Supplier supplier)
    {
        if (supplier == null)
            return Results.BadRequest("Не удалось добавить");

        bool res = _supplierService.Create(supplier);
        if (res == false)
        {
            return Results.BadRequest("Не удалось добавить!");
        }
        return Results.Ok("Успешно добавлено!");
    }


    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IResult Update([FromBody] Supplier supplier)
    {
        if (supplier == null)
            return Results.BadRequest("Не удалось обновить");

        bool res = _supplierService.Update(supplier);
        if (res == false)
        {
            return Results.BadRequest("Не удалось обновить!");
        }
        return Results.Ok("Успешно обновдено!");
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IResult Delete([FromRoute] int id)
    {

        bool res = _supplierService.Delete(id);
        if (res == false)
        {
            return Results.BadRequest("Не удалось удалить!");
        }
        return Results.Ok("Успешно удалено!");
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GetById([FromRoute] int id)
    {
        Supplier supplier = _supplierService.GetById(id);
        if (supplier == null)
            return Results.NotFound("Не удалось найти");

        return Results.Ok(supplier);
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GetAll()
    {
        IEnumerable<Supplier> suppliers = _supplierService.GetAll();
        if (suppliers == null)
            return Results.NotFound("Не удалось найти");

        return Results.Ok(suppliers);
    }

    [HttpGet("minProductQuantity={minQuantity}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GettingAListOfSuppliersWhoHaveProductsWithACertainQuantityInStock(int minQuantity)
    {
        IEnumerable<Supplier> suppliers = _supplierService.GettingAListOfSuppliersWhoHaveProductsWithACertainQuantityInStock(minQuantity);
        if (suppliers == null)
            return Results.NotFound("Не удалось найти");

        return Results.Ok(suppliers);
    }

}
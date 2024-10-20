

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/category/")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IResult Create([FromBody] Category category)
    {
        if (category == null)
            return Results.BadRequest("Не получилось добавить");

        bool res = _categoryService.Create(category);
        if (res == false)
            return Results.BadRequest("Не получилось добавить");

        return Results.Ok("Успешно добавлено");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IResult Delete([FromRoute] int id)
    {
        bool res = _categoryService.Delete(id);
        if (res == false)
            return Results.BadRequest("Не удалось удалить!");

        return Results.Ok("Успешно удалено!");
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IResult Update([FromBody] Category category)
    {
        if (category == null)
            return Results.BadRequest("Не получилось обновить!");

        bool res = _categoryService.Update(category);
        if (res == false)
            return Results.BadRequest("Не получилось обновить");

        return Results.Ok("Успешно обновлено!");
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GetById([FromRoute] int id)
    {
        Category category = _categoryService.GetById(id);
        if (category == null)
            return Results.NotFound("Не удалось найти!");

        return Results.Ok(category);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GetAll()
    {
        IEnumerable<Category> categories = _categoryService.GetAll();
        if (categories == null)
            return Results.NotFound("Не удалось найти!");

        return Results.Ok(categories);
    }


    [HttpGet("withProductCount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GettingACategoryWithTheNumberOfProductsInEachCategory()
    {
        var res = _categoryService.GettingACategoryWithTheNumberOfProductsInEachCategory();
        if (res == null)
            return Results.NotFound("Не удалось найти");

        return Results.Ok(res);
    }
}


using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/order/")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IResult Create(Order order)
    {
        if (order == null)
            return Results.BadRequest("Не получилось добавить");

        bool res = _orderService.Create(order);
        if (res == false)
            return Results.BadRequest("Не получилось добавить");

        return Results.Ok("Успешно добавлено");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IResult Delete([FromRoute] int id)
    {
        bool res = _orderService.Delete(id);
        if (res == false)
            return Results.BadRequest("Не удалось удалить!");

        return Results.Ok("Успешно удалено!");
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IResult Update([FromBody] Order order)
    {
        if (order == null)
            return Results.BadRequest("Не получилось обновить!");

        bool res = _orderService.Update(order);
        if (res == false)
            return Results.BadRequest("Не получилось обновить");

        return Results.Ok("Успешно обновлено!");
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GetById([FromRoute] int id)
    {
        Order order = _orderService.GetById(id);
        if (order == null)
            return Results.NotFound("Не удалось найти!");

        return Results.Ok(order);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GetAll()
    {
        IEnumerable<Order> orders = _orderService.GetAll();
        if (orders == null)
            return Results.NotFound("Не удалось найти!");

        return Results.Ok(orders);
    }

    // Query 2
    [HttpGet("supplierid={supplierid}-status={status}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GetAllOrdersForASpecificSupplierFilteredByStatus(int supplierid, string status)
    {
        IEnumerable<Order> orders = _orderService.GetAllOrdersForASpecificSupplierFilteredByStatus(supplierid, status);
        if (orders == null)
            return Results.NotFound("Не удалось найти!");

        return Results.Ok(orders);
    }


    // Query 5
    [HttpGet("startDate={startDate}&endDate={endDate}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult GetOrderInformationByDateRange(DateTime startDate, DateTime endDate)
    {
        IEnumerable<Order> orders = _orderService.GetOrderInformationByDateRange(startDate, endDate);
        if (orders == null)
            return Results.NotFound("Не удалось найти!");
        return Results.Ok(orders);
    }
}
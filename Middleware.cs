public class Middleware
{
    private readonly RequestDelegate _next;
    public Middleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {

        }
    }
}
public static class Extentions
{
    public static void AddApplicationServices(this IServiceCollection service)
    {
        service.AddTransient<IProductService, ProductService>();
        service.AddTransient<ICategoryService, CategoryService>();
        service.AddTransient<IOrderService, OrderService>();
        service.AddTransient<ISupplierService, SupplierService>();
    }
}
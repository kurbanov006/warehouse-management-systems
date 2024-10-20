var builder = WebApplication.CreateBuilder(args);

string path = "C:\\Users\\admin\\Desktop\\warehouse-management-systems\\appsettings.json";

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

IConfigurationRoot configuration = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile(path)
.Build();

builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ISupplierService, SupplierService>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();


app.Run();

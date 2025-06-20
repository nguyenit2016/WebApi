using Microsoft.EntityFrameworkCore;
using OrderServices.UnitOfWork;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

void RegisterServices(IServiceCollection services)
{
    var assemblies = new[] { Assembly.GetExecutingAssembly() };

    foreach (var type in assemblies.SelectMany(a => a.GetTypes()))
    {
        if (type.IsClass && !type.IsAbstract && type.Name.EndsWith("Repository"))
        {
            var interfaceType = type.GetInterfaces().FirstOrDefault(i => i.Name == $"I{type.Name}");
            if (interfaceType != null)
                services.AddScoped(interfaceType, type);
        }
        if (type.IsClass && !type.IsAbstract && type.Name.EndsWith("Service"))
        {
            var interfaceType = type.GetInterfaces().FirstOrDefault(i => i.Name == $"I{type.Name}");
            if (interfaceType != null)
                services.AddScoped(interfaceType, type);
        }
    }
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI DbContext
var connectionString = builder.Configuration.GetConnectionString("OrderDbConnection");
builder.Services.AddDbContext<OrderServices.Models.OrderDbContext>(options =>
   options.UseLazyLoadingProxies(false).UseSqlServer(connectionString));

// DI UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// DI Repositories and Services
RegisterServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

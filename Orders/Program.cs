using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Orders.BD;
using Orders.Entities;
using Orders.Interfaces;
using Orders.Repository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


#region OdataModel
static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<Order>("Order");
    builder.EntitySet<Country>("Country");
    return builder.GetEdmModel();
}
#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddOData(options => options
        .AddRouteComponents("odata", GetEdmModel())
        .Select()
        .Filter()
        .OrderBy()
        .SetMaxTop(20)
        .Count()
        .Expand()
    );

#region Repos
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
#endregion

#region DbContext

builder.Services.AddDbContext<OrderContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderCS"));
});

#endregion

#region CorsPolicy
builder.Services.AddCors(opt => opt.AddPolicy("CorsPolicy", c =>
{
    c.AllowAnyOrigin()
       .AllowAnyHeader()
       .AllowAnyMethod();
}));
#endregion


var app = builder.Build();

#region  _Migration

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrderContext>();
    context.Database.Migrate();
}
#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");


app.UseCors("CorsPolicy");

app.MapFallbackToFile("index.html"); ;

app.Run();

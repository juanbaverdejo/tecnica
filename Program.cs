using Microsoft.EntityFrameworkCore;
using PruebaTecnica.datos;
using PruebaTecnica.Migrations;
using PruebaTecnica.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Configuracion de la conexion a sql server

builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tarjeta}/{action=Home}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync(); // Aplica cualquier migración pendiente
    await ApplicationDbContextSeed.SeedAsync(dbContext); // Insertar datos de ejemplo si es necesario
}
app.Run();
public class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        DateTime fechaEspecifica = new DateTime(2027, 12, 30); // 30 de diciembre de 2027

        // Si no hay productos, agregar algunos datos iniciales
        if (!context.Tarjeta.Any())
        {
            var products = new List<Tarjeta>
            {
                new Tarjeta {NumeroTarjeta = "1111111111111111", PIN = "1111", Balance=10000,Bloqueada=false, IntentosFallidos=0,FechaVencimiento=fechaEspecifica},
                new Tarjeta {NumeroTarjeta = "1111222233334444", PIN = "1234", Balance=10000,Bloqueada=false, IntentosFallidos=0,FechaVencimiento=fechaEspecifica},
            };

            await context.Tarjeta.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}



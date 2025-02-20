using Microsoft.EntityFrameworkCore;
using PruebaTecnica.datos;

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

app.Run();

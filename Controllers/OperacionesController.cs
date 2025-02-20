using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Models;
using System;
using PruebaTecnica.datos; 

public class OperacionesController : Controller
{
    private readonly ApplicationDbContext _context;

    public OperacionesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Acción para mostrar la página de operaciones
    public IActionResult Index(int tarjetaId)
    {
        ViewBag.TarjetaId = tarjetaId;
        return View();
    }

    // Acción para mostrar el balance
    public IActionResult Balance(int tarjetaId)
    {
        // Buscar la tarjeta en la base de datos
        var tarjeta = _context.Tarjeta.Find(tarjetaId);

        if (tarjeta == null)
        {
            return RedirectToAction("Error", "Home", new { mensaje = "Tarjeta no encontrada." });
        }

        // Registrar la operación de balance
        var operacion = new Operacion
        {
            TarjetaId = tarjetaId,
            FechaHora = DateTime.Now,
            TipoOperacion = "Balance",
            Monto = tarjeta.Balance // No hay monto en una operación de balance
        };
        _context.Operaciones.Add(operacion);
        _context.SaveChanges();

        // Pasar el balance a la vista
        ViewBag.Balance = tarjeta.Balance;
        ViewBag.NumeroTarjeta = tarjeta.NumeroTarjeta;
        ViewBag.TarjetaId = tarjetaId;

        DateTime FechaVencimiento = tarjeta.FechaVencimiento;
        ViewBag.FechaVencimiento = FechaVencimiento.ToString("dd/MM/yyyy");

        return View();
    }

    // Acción para mostrar el formulario de retiro
    public IActionResult Retiro(int tarjetaId)
    {
        ViewBag.TarjetaId = tarjetaId;
        return View();
    }

    // Acción para procesar el retiro
    [HttpPost]
    public IActionResult RealizarRetiro(int tarjetaId, decimal monto)
    {
        // Buscar la tarjeta en la base de datos
        var tarjeta = _context.Tarjeta.Find(tarjetaId);

        if (tarjeta == null)
        {
            return RedirectToAction("Error", "Home", new { mensaje = "Tarjeta no encontrada." });
        }

        // Validar que el monto no exceda el balance
        if (monto > tarjeta.Balance)
        {
            TempData["MensajeError"] = "Fondos insuficientes.";
            return RedirectToAction("Retiro", new { tarjetaId });
        }

        // Registrar la operación de retiro
        var operacion = new Operacion
        {
            TarjetaId = tarjetaId,
            FechaHora = DateTime.Now,
            TipoOperacion = "Retiro",
            Monto = monto
        };
        _context.Operaciones.Add(operacion);

        // Actualizar el balance de la tarjeta
        tarjeta.Balance -= monto;
        _context.SaveChanges();

        // Redirigir a la página de reporte de operación
        return RedirectToAction("ReporteOperacion", new { tarjetaId, montoRetirado = monto });
    }

    // Acción para mostrar el reporte de operación
    public IActionResult ReporteOperacion(int tarjetaId, decimal montoRetirado)
    {
        // Buscar la tarjeta en la base de datos
        var tarjeta = _context.Tarjeta.Find(tarjetaId);

        if (tarjeta == null)
        {
            return RedirectToAction("Error", "Home", new { mensaje = "Tarjeta no encontrada." });
        }

        // Pasar los datos a la vista
        ViewBag.NumeroTarjeta = tarjeta.NumeroTarjeta;
        ViewBag.FechaHora = DateTime.Now;
        ViewBag.MontoRetirado = montoRetirado;
        ViewBag.BalanceActual = tarjeta.Balance;
        ViewBag.TarjetaId = tarjetaId;

        return View();
    }
}
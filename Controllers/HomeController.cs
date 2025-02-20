using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.datos;
using PruebaTecnica.Models;
using System;
using System.Diagnostics;

namespace PruebaTecnica.Controllers
{
    public class HomeController : Controller
    {
            private readonly ApplicationDbContext _context;

            public HomeController(ApplicationDbContext context)
            {
                _context = context;
            }

            // Acci�n para mostrar la vista de inicio
            public IActionResult Index()
            {
                return View();
            }

            // Acci�n para procesar el formulario
            [HttpPost]
            public IActionResult ValidarTarjeta(string numeroTarjeta, string pin)
            {
                // Validar la tarjeta en la base de datos
                var tarjeta = _context.Tarjeta
                    .FirstOrDefault(t => t.NumeroTarjeta == numeroTarjeta);
                // Eliminar guiones del n�mero de tarjeta
                numeroTarjeta = numeroTarjeta.Replace("-", "");

                // Validar que el n�mero de tarjeta tenga 16 d�gitos
                if (numeroTarjeta.Length != 16 || !numeroTarjeta.All(char.IsDigit))
            {
                ViewBag.MensajeError = "El n�mero de tarjeta debe tener 16 d�gitos.";
                return View("Index");
            }

            if (tarjeta == null || tarjeta.Bloqueada)
                {
                    // Mostrar mensaje de error si la tarjeta no es v�lida o est� bloqueada
                    ViewBag.MensajeError = "Tarjeta no v�lida o bloqueada.";
                    return View("Index");
                }

                // Validar el PIN
                if (tarjeta.PIN != pin)
                {
                    // Mostrar mensaje de error si el PIN es incorrecto
                    ViewBag.MensajeError = "PIN incorrecto.";
                    return View("Index");
                }

                // Redirigir a la p�gina de operaciones si la tarjeta y el PIN son v�lidos
                return RedirectToAction("Index", "Operacion", new { tarjetaId = tarjeta.Id });
            }
        }
    }

using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.datos;
using System;

namespace PruebaTecnica.Controllers
{
        public class TarjetaController : Controller
        {
            private readonly ApplicationDbContext _context;

            public TarjetaController(ApplicationDbContext context)
            {
                _context = context;
            }
            public IActionResult Home()
            {
                return View();
            }
        [HttpPost]
            public IActionResult ValidarTarjeta(string numeroTarjeta)
            {
                // Eliminar guiones del número de tarjeta
                numeroTarjeta = numeroTarjeta.Replace("-", "");

                // Validar que el número de tarjeta tenga 16 dígitos
                if (numeroTarjeta.Length != 16 || !numeroTarjeta.All(char.IsDigit))
                {
                    ViewBag.MensajeError = "El número de tarjeta debe tener 16 dígitos.";
                    return RedirectToAction("Error", new { mensaje = "Tarjeta no válida o bloqueada." });
            }
            var tarjeta = _context.Tarjeta
                    .FirstOrDefault(t => t.NumeroTarjeta == numeroTarjeta);
                
                if (tarjeta == null || tarjeta.Bloqueada)
                {
                    return RedirectToAction("Error", new { mensaje = "Tarjeta no válida o bloqueada." });
                }

                return RedirectToAction("IngresarPIN", new { tarjetaId = tarjeta.Id });
            }

            public IActionResult IngresarPIN(int tarjetaId)
            {
                ViewBag.TarjetaId = tarjetaId;
                return View();
            }

            [HttpPost]
            public IActionResult ValidarPIN(int tarjetaId, string pin)
            {
                var tarjeta = _context.Tarjeta.Find(tarjetaId);

                if (tarjeta == null || tarjeta.PIN != pin)
                {
                    tarjeta.IntentosFallidos++;
                    if (tarjeta.IntentosFallidos >= 4)
                    {
                        tarjeta.Bloqueada = true;
                    }
                    _context.SaveChanges();

                    return RedirectToAction("Error", new { mensaje = "PIN incorrecto." });
                }
                tarjeta.IntentosFallidos = 0;
                _context.SaveChanges();
                return RedirectToAction("Index", "Operaciones", new { tarjetaId });
            }

            public IActionResult Error(string mensaje)
            {
                ViewBag.Mensaje = mensaje;
                return View();
            }
        }
    }

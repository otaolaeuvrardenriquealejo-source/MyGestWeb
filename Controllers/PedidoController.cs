using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyGestWeb.Data;
using MyGestWeb.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyGestWeb.Controllers;

public class PedidosController(ApplicationDbContext context) : Controller
{
    // GET: Pedidos/Index (Solo para que no de error si vas a la lista)
    public async Task<IActionResult> Index()
    {
        var pedidos = await context.Pedidos.OfType<PedidoSimple>().Include(p => p.CuentaPago).ThenInclude(c => c.Cliente).ToListAsync();
        return View(pedidos);
    }

    // GET: Pedidos/Create (Carga la pantalla de la cabecera)
    public async Task<IActionResult> Create()
    {
        // 1. Buscamos todas las cuentas e incluimos los datos de su cliente dueño
        var cuentas = await context.Cuentas.Include(c => c.Cliente).ToListAsync();

        // 2. Armamos el texto combinando Cliente + ID Cuenta + Saldo
        var listaCuentas = cuentas.Select(c => new
        {
            Id = c.Id,
            Descripcion = $"{c.Cliente?.Nombre} - Cuenta #{c.Id} (Saldo: ${c.SaldoDisponible})"
        });

        // 3. Enviamos la lista a la vista mediante un SelectList en el ViewBag
        ViewBag.CuentasDisponibles = new SelectList(listaCuentas, "Id", "Descripcion");

        return View();
    }

    // POST: Pedidos/Create (Guarda la cabecera en la BD)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int CuentaId)
    {
        if (CuentaId <= 0)
        {
            return BadRequest("Debe seleccionar una cuenta.");
        }

        // Creamos el pedido solo con la cuenta y datos por defecto
        var nuevoPedido = new PedidoSimple
        {
            CuentaId = CuentaId,
            Estado = "Pendiente",
            Fecha = DateTime.Now
        };

        context.Add(nuevoPedido);
        await context.SaveChangesAsync();

        // ÉXITO: El próximo paso lógico será redirigirlo al "Carrito" para agregar productos.
        // Por ahora lo mandamos al Index.
        return RedirectToAction(nameof(Index));
    }
}
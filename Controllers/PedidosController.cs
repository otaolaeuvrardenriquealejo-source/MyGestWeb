using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyGestWeb.Data;
using MyGestWeb.Models;

namespace MyGestWeb.Controllers;

public class PedidosController(ApplicationDbContext context) : Controller
{
    // 1. PANTALLA PRINCIPAL: Listado de pedidos
    public async Task<IActionResult> Index()
    {
        var pedidos = await context.Pedidos
            .OfType<PedidoSimple>()
            .Include(p => p.CuentaPago)
                .ThenInclude(c => c.Cliente) // Incluimos el cliente para mostrar su nombre
            .ToListAsync();

        return View(pedidos);
    }

    // 2. CARGAR FORMULARIO: Envía solo la lista de Clientes al iniciar
    public async Task<IActionResult> Create()
    {
        var clientes = await context.Clientes.ToListAsync();
        ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre");

        return View();
    }

    // 3. ENDPOINT AJAX: El navegador llama aquí para buscar cuentas de un cliente
    [HttpGet]
    public async Task<JsonResult> ObtenerCuentas(int clienteId)
    {
        var cuentas = await context.Cuentas
            .Where(c => c.ClienteId == clienteId)
            .Select(c => new
            {
                value = c.Id,
                text = $"Cuenta #{c.Id} (Saldo: ${c.SaldoDisponible})"
            })
            .ToListAsync();

        return Json(cuentas);
    }

    // 4. GUARDAR FORMULARIO: Recibe la cuenta y crea el pedido en SQLite
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int CuentaId)
    {
        if (CuentaId <= 0)
        {
            ModelState.AddModelError("", "Debe seleccionar una cuenta válida.");
            ViewBag.Clientes = new SelectList(await context.Clientes.ToListAsync(), "Id", "Nombre");
            return View();
        }

        var nuevoPedido = new PedidoSimple
        {
            CuentaId = CuentaId,
            Estado = "Pendiente",
            Fecha = DateTime.Now
        };

        context.Add(nuevoPedido);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
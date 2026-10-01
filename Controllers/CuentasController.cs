using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyGestWeb.Data;
using MyGestWeb.Models;

public class CuentasController : Controller
{
    private readonly ApplicationDbContext _context;

    public CuentasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CUENTAS
    public async Task<IActionResult> Index()    
    {
        // El .Include le dice a la base de datos: "Traé la cuenta y traé también al Cliente vinculado"
        var cuentas = await _context.Cuentas.Include(c => c.Cliente).ToListAsync();
        return View(cuentas);
    }

    // GET: CUENTAS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cuenta = await _context.Cuentas
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cuenta == null)
        {
            return NotFound();
        }

        return View(cuenta);
    }

    // GET: CUENTAS/Create
    public IActionResult Create()
    {
        ViewBag.ListaClientes = _context.Clientes.ToList();
        return View();
    }

    // POST: CUENTAS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,SaldoDisponible,TratamientoEspecifico,ClienteId,Cliente,PedidosPagados")] Cuenta cuenta)
    {
        if (ModelState.IsValid)
        {
            _context.Add(cuenta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nombre", cuenta.ClienteId);
        return View(cuenta);
    }

    // GET: CUENTAS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cuenta = await _context.Cuentas.FindAsync(id);
        if (cuenta == null)
        {
            return NotFound();
        }
        return View(cuenta);
    }

    // POST: CUENTAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,SaldoDisponible,TratamientoEspecifico,ClienteId,Cliente,PedidosPagados")] Cuenta cuenta)
    {
        if (id != cuenta.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cuenta);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuentaExists(cuenta.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(cuenta);
    }

    // GET: CUENTAS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cuenta = await _context.Cuentas
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cuenta == null)
        {
            return NotFound();
        }

        return View(cuenta);
    }

    // POST: CUENTAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var cuenta = await _context.Cuentas.FindAsync(id);
        if (cuenta != null)
        {
            _context.Cuentas.Remove(cuenta);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CuentaExists(int? id)
    {
        return _context.Cuentas.Any(e => e.Id == id);
    }
    // GET: Cuentas/AumentarSaldo/5
    public async Task<IActionResult> AumentarSaldo(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cuenta = await _context.Cuentas.FindAsync(id);
        if (cuenta == null)
        {
            return NotFound();
        }

        return View(cuenta);
    }

    // POST: Cuentas/AumentarSaldo/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AumentarSaldo(int id, decimal monto)
    {
        if (monto <= 0)
        {
            ModelState.AddModelError("monto", "El monto a ingresar debe ser mayor a 0.");
        }

        var cuenta = await _context.Cuentas.FindAsync(id);
        if (cuenta == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Si en tu modelo la propiedad se llama distinto (ej. SaldoDisponible), ajustá el nombre acá
            cuenta.SaldoDisponible += monto;

            _context.Update(cuenta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(cuenta);
    }
}

using Microsoft.EntityFrameworkCore;
using MyGestWeb.Models;

namespace MyGestWeb.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    // Estas propiedades representan las tablas físicas en la base de datos
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Cuenta> Cuentas { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<DetallePedido> DetallesPedidos { get; set; }
    public DbSet<OrdenDistribucion> OrdenesDistribucion { get; set; }
}
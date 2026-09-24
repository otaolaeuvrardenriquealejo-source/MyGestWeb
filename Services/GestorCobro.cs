using MyGestWeb.Data; // Asume que tu DbContext está en la carpeta Data
using MyGestWeb.Models;
using System;
using System.Linq;

namespace MyGestWeb.Services;

// ¡CONSTRUCTOR PRIMARIO DE .NET 10! 
// "context" queda inyectado y disponible para usar en toda la clase.
public class GestorCobroService(ApplicationDbContext context)
{
    public void CobrarPedidosDiarios()
    {
        // 1. Usamos "context" directamente para buscar los pedidos pendientes
        var pedidosPendientes = context.Pedidos
            .OfType<PedidoSimple>() // Filtramos solo los simples para procesarlos
            .Where(p => p.Estado == "Pendiente")
            .ToList();

        foreach (var pedido in pedidosPendientes)
        {
            if (pedido.EsCobrable())
            {
                ConfirmarPedido(pedido);
                GenerarOrdenDistribucion(pedido);
            }
            else
            {
                pedido.Estado = "Rechazado";
            }
        }

        // 2. Guardamos todos los cambios en la base de datos de una sola vez
        context.SaveChanges();
    }

    private void ConfirmarPedido(PedidoSimple pedido)
    {
        decimal total = pedido.CalcularTotal();

        if (pedido.CuentaPago != null)
        {
            pedido.CuentaPago.DescontarSaldo(total);
        }

        foreach (var detalle in pedido.Detalles)
        {
            if (detalle.Producto != null)
            {
                detalle.Producto.DescontarStock(detalle.Cantidad);
            }
        }

        pedido.Estado = "Listo de reparto";
    }

    private void GenerarOrdenDistribucion(PedidoSimple pedido)
    {
        var orden = new OrdenDistribucion
        {
            NumeroOrden = new Random().Next(1000, 9999),
            Estado = "En camino"
        };

        // Agregamos la orden a la base de datos
        context.Add(orden);
        pedido.OrdenDistribucion = orden;
    }
}
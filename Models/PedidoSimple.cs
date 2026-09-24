using System.Collections.Generic;
using System.Linq;

namespace MyGestWeb.Models;

public class PedidoSimple : Pedido
{
    public int CuentaId { get; set; }
    public Cuenta? CuentaPago { get; set; }

    public List<DetallePedido> Detalles { get; set; } = [];

    public int? OrdenDistribucionId { get; set; }
    public OrdenDistribucion? OrdenDistribucion { get; set; }

    public int? PedidoCompuestoId { get; set; }
    public PedidoCompuesto? PedidoCompuestoPertenece { get; set; }

    public override decimal CalcularTotal() => Detalles.Sum(d => d.Subtotal());

    public override bool EsCobrable()
    {
        if (CuentaPago == null) return false;

        bool haySaldo = CuentaPago.SaldoDisponible >= CalcularTotal();
        // Validamos que exista el producto y haya stock
        bool hayStock = Detalles.All(d => d.Producto != null && d.Producto.HayStockDisponible(d.Cantidad));

        return haySaldo && hayStock;
    }
}
namespace MyGestWeb.Models;

public class DetallePedido
{
    public int Id { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitarioHistorico { get; set; }

    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }

    public int PedidoSimpleId { get; set; }
    public PedidoSimple? PedidoSimple { get; set; }

    public decimal Subtotal() => Cantidad * PrecioUnitarioHistorico;
}

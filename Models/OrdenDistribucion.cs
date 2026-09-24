namespace MyGestWeb.Models;

public class OrdenDistribucion
{
    public int Id { get; set; }
    public int NumeroOrden { get; set; }
    public required string Estado { get; set; }

    public PedidoSimple? PedidoAsociado { get; set; }

    public void MarcarComoEntregado()
    {
        Estado = "Entregado";
        if (PedidoAsociado != null)
        {
            PedidoAsociado.Estado = "Entregado";
        }
    }
}

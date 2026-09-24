namespace MyGestWeb.Models;

public class Producto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public int Stock { get; set; }
    public decimal Precio { get; set; }

    public bool HayStockDisponible(int cantidadSolicitada) => Stock >= cantidadSolicitada;

    public void DescontarStock(int cantidadVendida) => Stock -= cantidadVendida;
}

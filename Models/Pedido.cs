using System;

namespace MyGestWeb.Models;

public abstract class Pedido
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public required string Estado { get; set; } = "Pendiente";

    public abstract decimal CalcularTotal();
    public abstract bool EsCobrable();
}

using System.Collections.Generic;
using System.Linq;

namespace MyGestWeb.Models;

public class PedidoCompuesto : Pedido
{
    public List<PedidoSimple> PedidosInternos { get; set; } = [];

    public override decimal CalcularTotal() => PedidosInternos.Sum(p => p.CalcularTotal());

    public override bool EsCobrable() => PedidosInternos.All(p => p.EsCobrable());
}

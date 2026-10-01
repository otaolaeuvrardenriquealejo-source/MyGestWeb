using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyGestWeb.Models;

public class Cuenta
{
    public int Id { get; set; }
    [Display(Name = "Saldo Disponible")]
    public decimal SaldoDisponible { get; set; }
    [Display(Name = "Tratamiento Específico")]
    public required string TratamientoEspecifico { get; set; }

    // Clave foránea
    [Display(Name = "ID Cliente")]
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    public List<PedidoSimple> PedidosPagados { get; set; } = [];

    // Métodos con sintaxis moderna (expression-bodied)
    public void AumentarSaldo(decimal monto) => SaldoDisponible += monto;

    public bool DescontarSaldo(decimal monto)
    {
        if (SaldoDisponible >= monto)
        {
            SaldoDisponible -= monto;
            return true;
        }
        return false;
    }
}

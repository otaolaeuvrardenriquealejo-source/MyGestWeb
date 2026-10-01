using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyGestWeb.Models;

public class Cliente
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    [Display(Name = "Dirección")]
    public required string Direccion { get; set; }

    // Propiedad de navegación
    public List<Cuenta> Cuentas { get; set; } = [];

    public void AumentarSaldo(Cuenta cuenta, decimal monto)
    {
        if (monto > 0 && cuenta != null && Cuentas.Contains(cuenta))
        {
            cuenta.AumentarSaldo(monto);
        }
    }
}

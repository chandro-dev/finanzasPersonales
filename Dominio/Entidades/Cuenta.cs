using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Cuenta
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal SaldoInicial { get; set; }
        public required string Tipo { get; set; } // banco, efectivo
        public ICollection<Transaccion>? Transacciones { get; set; }

    }
}

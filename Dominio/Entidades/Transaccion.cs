using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dominio.Entidades
{
    public class Transaccion
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Descripcion { get; set; } = string.Empty;

        public int CuentaId { get; set; }
        public Cuenta? Cuenta { get; set; }

        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public bool EsAutomatica { get; set; }
    }
}

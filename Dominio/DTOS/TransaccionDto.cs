using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.DTOS
{
    public class TransaccionDto
    {
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int CuentaId { get; set; }
        public int CategoriaId { get; set; }
        public bool EsAutomatica { get; set; } = false;
    } 
}

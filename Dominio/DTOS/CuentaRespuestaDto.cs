using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.DTOS
{
    public class CuentaRespuestaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal SaldoInicial { get; set; }
        public string Tipo { get; set; } = null!;
    }

}

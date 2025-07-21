using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.DTOS
{
    public class CategoriaDto
    {
        public string Nombre { get; set; } = string.Empty;
        public bool EsIngreso { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.DTOS
{
    public class CategoriaRespuestaDto
    {
        public int Id {  get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool EsIngreso { get; set; }
    }
}

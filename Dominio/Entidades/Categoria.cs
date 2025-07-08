using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades;

public class Categoria
{
    public int Id { get; set; } // ← esta es la clave primaria
    public string Nombre { get; set; } = string.Empty;
    public bool EsIngreso { get; set; }
}
    
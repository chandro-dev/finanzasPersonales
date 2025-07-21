using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;

namespace Dominio.Entidades
{
    public class ProgramacionAutomatica
    {
        public int Id { get; set; }

        public int CuentaId { get; set; }
        public Cuenta? Cuenta { get; set; }

        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = string.Empty;

        public string CronExpresion { get; set; } = Cron.Daily(); // Hangfire cron

        public bool Activa { get; set; } = true;
    }

}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidades;
namespace Persistencia
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cuenta> Cuentas => Set<Cuenta>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Transaccion> Transacciones => Set<Transaccion>();
        public DbSet<ProgramacionAutomatica> ProgramacionAutomaticas => Set<ProgramacionAutomatica>();

    }
}

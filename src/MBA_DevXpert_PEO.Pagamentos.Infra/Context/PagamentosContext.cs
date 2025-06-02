using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Core.Data;
using MBA_DevXpert_PEO.Core.Messages;
using MBA_DevXpert_PEO.Pagamentos.Domain.Entities;
using MBA_DevXpert_PEO.Pagamentos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MBA_DevXpert_PEO.Pagamentos.Infra.Context
{
    public class PagamentosContext : DbContext, IUnitOfWork
    {
        public PagamentosContext(DbContextOptions<PagamentosContext> options)
        : base(options) { }

        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model
                .GetEntityTypes().SelectMany(e => e.GetProperties()
                .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentosContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}

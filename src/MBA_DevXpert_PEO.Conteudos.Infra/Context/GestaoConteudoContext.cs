using MBA_DevXpert_PEO.Core.Data;
using MBA_DevXpert_PEO.Core.Messages;
using MBA_DevXpert_PEO.Conteudos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MBA_DevXpert_PEO.Conteudos.Infra.Context
{
    public class GestaoConteudoContext : DbContext, IUnitOfWork
    {
        public GestaoConteudoContext(DbContextOptions<GestaoConteudoContext> options)
            : base(options) { }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Aula> Aulas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model
                .GetEntityTypes().SelectMany(e => e.GetProperties()
                .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GestaoConteudoContext).Assembly);

            modelBuilder.Entity<Curso>()
            .HasMany(c => c.Aulas)
            .WithOne(a => a.Curso)
            .HasForeignKey(a => a.CursoId)
            .OnDelete(DeleteBehavior.Cascade);

        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}

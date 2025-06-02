using System.Collections.Generic;
using System.Reflection.Emit;
using MBA_DevXpert_PEO.Core.Data;
using MBA_DevXpert_PEO.Core.Messages;
using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MBA_DevXpert_PEO.Alunos.Infra.Context
{
    public class AlunosContext : DbContext, IUnitOfWork
    {
        public AlunosContext(DbContextOptions<AlunosContext> options)
            : base(options) { }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Certificado> Certificados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model
                .GetEntityTypes().SelectMany(e => e.GetProperties()
                .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlunosContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}

using System.Collections.Generic;
using System.Reflection.Emit;
using MBA_DevXpert_PEO.Core.Data;
using MBA_DevXpert_PEO.Core.Messages;
using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MBA_DevXpert_PEO.Core.Communication.Mediator;

namespace MBA_DevXpert_PEO.Alunos.Infra.Context
{
    public class AlunosContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public AlunosContext(DbContextOptions<AlunosContext> options, IMediatorHandler mediatorHandler)
        : base(options)
        {
            _mediatorHandler = mediatorHandler;
        }

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
            var result = await base.SaveChangesAsync();

            Console.WriteLine($"Commit result: {result}");
            foreach (var entry in ChangeTracker.Entries())
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
            }

            return result > 0;
        }

    }
}

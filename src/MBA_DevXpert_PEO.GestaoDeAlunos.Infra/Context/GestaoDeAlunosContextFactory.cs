using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MBA_DevXpert_PEO.GestaoDeAlunos.Infra.Context
{
    public class GestaoDeAlunosContextFactory : IDesignTimeDbContextFactory<GestaoDeAlunosContext>
    {
        public GestaoDeAlunosContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<GestaoDeAlunosContext>();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            return new GestaoDeAlunosContext(optionsBuilder.Options);
        }
    }
}

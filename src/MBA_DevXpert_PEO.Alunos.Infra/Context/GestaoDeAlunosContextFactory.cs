using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MBA_DevXpert_PEO.Alunos.Infra.Context
{
    public class AlunosContextFactory : IDesignTimeDbContextFactory<AlunosContext>
    {
        public AlunosContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AlunosContext>();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            return new AlunosContext(optionsBuilder.Options);
        }
    }
}

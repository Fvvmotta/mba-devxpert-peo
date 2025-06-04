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
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AlunosContext>();
            var connection = configuration.GetConnectionString("DefaultConnection");

            if (environment == "Development")
            {
                optionsBuilder.UseSqlServer(connection,
                    x => x.MigrationsAssembly("MBA_DevXpert_PEO.Alunos.Infra"));
            }
            else
            {
                optionsBuilder.UseSqlite(connection,
                    x => x.MigrationsAssembly("MBA_DevXpert_PEO.Alunos.Infra"));
            }

            return new AlunosContext(optionsBuilder.Options);
        }
    }

}

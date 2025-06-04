using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MBA_DevXpert_PEO.Conteudos.Infra.Context
{
    public class GestaoConteudoContextFactory : IDesignTimeDbContextFactory<GestaoConteudoContext>
    {
        public GestaoConteudoContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<GestaoConteudoContext>();
            var connection = configuration.GetConnectionString("DefaultConnection");

            if (environment == "Development")
            {
                optionsBuilder.UseSqlServer(connection,
                    x => x.MigrationsAssembly("MBA_DevXpert_PEO.Conteudos.Infra"));
            }
            else
            {
                optionsBuilder.UseSqlite(connection,
                    x => x.MigrationsAssembly("MBA_DevXpert_PEO.Conteudos.Infra"));
            }

            return new GestaoConteudoContext(optionsBuilder.Options);
        }
    }

}

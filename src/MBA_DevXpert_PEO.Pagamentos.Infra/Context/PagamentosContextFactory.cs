using MBA_DevXpert_PEO.Pagamentos.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MBA_DevXpert_PEO.Pagamentos.Infra.Context
{
    public class PagamentosContextFactory : IDesignTimeDbContextFactory<PagamentosContext>
    {
        public PagamentosContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PagamentosContext>();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            return new PagamentosContext(optionsBuilder.Options);
        }
    }
}

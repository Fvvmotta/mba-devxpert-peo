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
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<GestaoConteudoContext>();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            return new GestaoConteudoContext(optionsBuilder.Options);
        }
    }
}

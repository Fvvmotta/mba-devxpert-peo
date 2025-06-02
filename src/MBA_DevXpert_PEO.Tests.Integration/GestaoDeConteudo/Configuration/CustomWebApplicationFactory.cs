using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MBA_DevXpert_PEO.Conteudos.Infra.Context;
using System.IO;
using System.Linq;

namespace MBA_DevXpert_PEO.Tests.Integration.Conteudos.Configuration
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json");
            });

            builder.ConfigureServices(services =>
            {
                // Remove o contexto real
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<GestaoConteudoContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Recarrega o IConfiguration para pegar a connection string do appsettings.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                // Registra o contexto com o arquivo físico SQLite
                services.AddDbContext<GestaoConteudoContext>(options =>
                {
                    options.UseSqlite(connectionString);
                });

                // Aplica as migrations se necessário
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<GestaoConteudoContext>();
                    db.Database.Migrate(); // Aplica as migrations corretamente
                }
            });
        }
    }
}

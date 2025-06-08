using MBA_DevXpert_PEO.Alunos.Infra.Context;
using MBA_DevXpert_PEO.Api.Identity;
using MBA_DevXpert_PEO.Conteudos.Infra.Context;
using MBA_DevXpert_PEO.Pagamentos.Infra.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DatabaseSelectorExtension
{
    public static void AddDatabaseSelector(this WebApplicationBuilder builder)
    {
        Console.WriteLine($"Ambiente atual: {builder.Environment.EnvironmentName}");
        var services = builder.Services;
        var configuration = builder.Configuration;
        var environment = builder.Environment;

        var connection = configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"Connection String: {connection}");
        var useSqlite = environment.IsDevelopment() || environment.IsEnvironment("Testing");

        RegisterDbContext<IdentityContext>(services, connection, useSqlite);
        RegisterDbContext<AlunosContext>(services, connection, useSqlite);
        RegisterDbContext<GestaoConteudoContext>(services, connection, useSqlite);
        RegisterDbContext<PagamentosContext>(services, connection, useSqlite);
    }

    private static void RegisterDbContext<T>(IServiceCollection services, string connection, bool useSqlite) where T : DbContext
    {
        if (useSqlite)
            services.AddDbContext<T>(options => options.UseSqlite(connection));
            //services.AddDbContext<T>(options => options.UseSqlServer(connection).EnableSensitiveDataLogging().LogTo(Console.WriteLine));
        else
            services.AddDbContext<T>(options => options.UseSqlServer(connection));
    }
}
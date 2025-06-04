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
        var useSqlServer = environment.IsDevelopment();

        RegisterDbContext<IdentityContext>(services, connection, useSqlServer);
        RegisterDbContext<AlunosContext>(services, connection, useSqlServer);
        RegisterDbContext<GestaoConteudoContext>(services, connection, useSqlServer);
        RegisterDbContext<PagamentosContext>(services, connection, useSqlServer);
    }

    private static void RegisterDbContext<T>(IServiceCollection services, string connection, bool useSqlServer) where T : DbContext
    {
        if (useSqlServer)
            services.AddDbContext<T>(options => options.UseSqlServer(connection));
        else
            services.AddDbContext<T>(options => options.UseSqlite(connection));
    }
}
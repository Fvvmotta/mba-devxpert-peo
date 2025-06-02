using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MBA_DevXpert_PEO.Conteudos.Infra.Context;
using MBA_DevXpert_PEO.Alunos.Infra.Context;
using MBA_DevXpert_PEO.Pagamentos.Infra.Context;

namespace MBA_DevXpert_PEO.Api.Configuration
{
    public static class InfraDependencyInjection
    {
        public static IServiceCollection RegisterInfraData(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<GestaoConteudoContext>(options =>
                options.UseSqlite(connectionString));

            services.AddDbContext<AlunosContext>(options =>
                options.UseSqlite(connectionString));

            services.AddDbContext<PagamentosContext>(options =>
                options.UseSqlite(connectionString));

            return services;
        }
    }
}

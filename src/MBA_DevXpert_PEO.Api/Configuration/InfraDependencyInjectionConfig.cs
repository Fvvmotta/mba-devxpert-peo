using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MBA_DevXpert_PEO.GestaoDeConteudo.Infra.Context;
using MBA_DevXpert_PEO.GestaoDeAlunos.Infra.Context;
using MBA_DevXpert_PEO.PagamentoEFaturamento.Infra.Context;

namespace MBA_DevXpert_PEO.Api.Configuration
{
    public static class InfraDependencyInjection
    {
        public static IServiceCollection RegisterInfraData(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<GestaoConteudoContext>(options =>
                options.UseSqlite(connectionString));

            services.AddDbContext<GestaoDeAlunosContext>(options =>
                options.UseSqlite(connectionString));

            services.AddDbContext<PagamentosContext>(options =>
                options.UseSqlite(connectionString));

            return services;
        }
    }
}

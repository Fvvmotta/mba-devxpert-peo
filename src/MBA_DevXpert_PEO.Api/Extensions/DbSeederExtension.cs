using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MBA_DevXpert_PEO.GestaoDeConteudo.Infra.Context;
using MBA_DevXpert_PEO.GestaoDeConteudo.Domain.Entities;
using MBA_DevXpert_PEO.GestaoDeConteudo.Domain.ValueObjects;
using MBA_DevXpert_PEO.GestaoDeAlunos.Infra.Context;
using MBA_DevXpert_PEO.PagamentoEFaturamento.Domain.Entities;
using MBA_DevXpert_PEO.PagamentoEFaturamento.Domain.ValueObjects;
using MBA_DevXpert_PEO.PagamentoEFaturamento.Infra.Context;
using MBA_DevXpert_PEO.GestaoDeAlunos.Domain.Entities;
using MBA_DevXpert_PEO.GestaoDeAlunos.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using MBA_DevXpert_PEO.Api.Identity;


namespace MBA_DevXpert_PEO.Api.Extensions
{
    public static class DbSeederExtension
    {
        public static async Task UseDatabaseSeeder(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var conteudoContext = services.GetRequiredService<GestaoConteudoContext>();
            var alunosContext = services.GetRequiredService<GestaoDeAlunosContext>();
            var pagamentosContext = services.GetRequiredService<PagamentosContext>();
            var identityContext = services.GetRequiredService<IdentityContext>();

            await conteudoContext.Database.MigrateAsync();
            await alunosContext.Database.MigrateAsync();
            await pagamentosContext.Database.MigrateAsync();
            await identityContext.Database.MigrateAsync();

            await SeedCursos(conteudoContext);
            await app.SeedBaseDeTesteCompleta();
        }

        private static async Task SeedCursos(GestaoConteudoContext context)
        {
            if (context.Cursos.Any())
                return;

            var curso = new Curso(
                nome: "Curso de ASP.NET Core",
                cargaHoraria: 40,
                autor: "Fernando Motta",
                conteudoProgramatico: ConteudoProgramatico.Criar("MVC, DI, EF Core, Swagger, Docker")
            );

            curso.AdicionarAula("Introdução", "Conceitos iniciais", "https://material.com/1");
            curso.AdicionarAula("Middlewares", "Ciclo de requisição", "https://material.com/2");
            curso.AdicionarAula("Entity Framework", "Banco de dados com EF", "https://material.com/3");

            context.Cursos.Add(curso);
            await context.SaveChangesAsync();
        }
        private static async Task SeedBaseDeTesteCompleta(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var conteudoContext = scope.ServiceProvider.GetRequiredService<GestaoConteudoContext>();
            var alunosContext = scope.ServiceProvider.GetRequiredService<GestaoDeAlunosContext>();
            var pagamentosContext = scope.ServiceProvider.GetRequiredService<PagamentosContext>();

            var curso = await conteudoContext.Cursos.Include(c => c.Aulas).FirstOrDefaultAsync();
            if (curso == null) return;

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var alunoRepository = scope.ServiceProvider.GetRequiredService<IAlunoRepository>();

            // Admin
            const string adminEmail = "admin@teste.com";
            const string adminPassword = "Admin@123";

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));

            if (await userManager.FindByEmailAsync(adminEmail) is null)
            {
                var adminUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, adminPassword);
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            //Aluno
            const string alunoEmail = "aluno@teste.com";
            const string alunoPassword = "Aluno@123";
            Guid alunoId;

            if (!await roleManager.RoleExistsAsync("Aluno"))
                await roleManager.CreateAsync(new IdentityRole<Guid>("Aluno"));

            var user = await userManager.FindByEmailAsync(alunoEmail);
            if (user is null)
            {
                alunoId = Guid.NewGuid();

                var alunoUser = new ApplicationUser
                {
                    Id = alunoId,
                    UserName = alunoEmail,
                    Email = alunoEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(alunoUser, alunoPassword);
                await userManager.AddToRoleAsync(alunoUser, "Aluno");

                var alunoDomain = Aluno.CriarComId(alunoId, "Aluno Teste", alunoEmail);
                alunoRepository.Adicionar(alunoDomain);
                await alunoRepository.UnitOfWork.Commit();
            }
            else
            {
                alunoId = user.Id;
            }

            var aluno = await alunosContext.Alunos
            .Include(a => a.Matriculas)
            .FirstOrDefaultAsync(a => a.Id == alunoId);

            var matricula = new Matricula(curso.Id);
            aluno.Matricular(matricula);

            matricula.DefinirTotalAulas(curso.Aulas.Count);
            for (int i = 0; i < curso.Aulas.Count; i++)
            {
                matricula.RegistrarAulaConcluida();
            }
            matricula.Concluir();

            alunosContext.Matriculas.Add(matricula);
            await alunosContext.SaveChangesAsync();


            var cartao = new DadosCartao("Aluno Teste", "4111111111111111", "12/30", "123");
            var pagamento = new Pagamento(matricula.Id, 499.90m, cartao);
            pagamento.Confirmar();
            pagamentosContext.Pagamentos.Add(pagamento);
            await pagamentosContext.SaveChangesAsync();
        }
    }
}

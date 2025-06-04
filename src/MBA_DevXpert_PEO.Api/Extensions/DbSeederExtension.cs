using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MBA_DevXpert_PEO.Conteudos.Infra.Context;
using MBA_DevXpert_PEO.Conteudos.Domain.Entities;
using MBA_DevXpert_PEO.Conteudos.Domain.ValueObjects;
using MBA_DevXpert_PEO.Alunos.Infra.Context;
using MBA_DevXpert_PEO.Pagamentos.Domain.Entities;
using MBA_DevXpert_PEO.Pagamentos.Domain.ValueObjects;
using MBA_DevXpert_PEO.Pagamentos.Infra.Context;
using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using MBA_DevXpert_PEO.Alunos.Domain.Repositories;
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

            Console.WriteLine($"Ambiente: {app.Environment.EnvironmentName}");

            var conteudoContext = services.GetRequiredService<GestaoConteudoContext>();
            var alunosContext = services.GetRequiredService<AlunosContext>();
            var pagamentosContext = services.GetRequiredService<PagamentosContext>();
            var identityContext = services.GetRequiredService<IdentityContext>();

            await conteudoContext.Database.MigrateAsync();
            await alunosContext.Database.MigrateAsync();
            await pagamentosContext.Database.MigrateAsync();
            await identityContext.Database.MigrateAsync();

            await SeedCursos(conteudoContext);
            await SeedBaseDeTesteCompleta(services);
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

        private static async Task SeedBaseDeTesteCompleta(IServiceProvider services)
        {
            var conteudoContext = services.GetRequiredService<GestaoConteudoContext>();
            var alunosContext = services.GetRequiredService<AlunosContext>();
            var pagamentosContext = services.GetRequiredService<PagamentosContext>();

            var curso = await conteudoContext.Cursos.Include(c => c.Aulas).FirstOrDefaultAsync();
            if (curso == null) return;

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var alunoRepository = services.GetRequiredService<IAlunoRepository>();

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

            // Aluno
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

            bool jaMatriculado = aluno.Matriculas.Any(m => m.CursoId == curso.Id);
            if (!jaMatriculado)
            {
                var matricula = new Matricula(curso.Id);
                aluno.Matricular(matricula);

                matricula.DefinirTotalAulas(curso.Aulas.Count);
                for (int i = 0; i < curso.Aulas.Count; i++)
                {
                    matricula.RegistrarAulaConcluida();
                }

                matricula.Concluir(
                    nomeAluno: aluno.Nome,
                    nomeCurso: curso.Nome,
                    cargaHorariaCurso: curso.CargaHoraria,
                    dataConclusao: DateTime.UtcNow
                );

                alunosContext.Matriculas.Add(matricula);
                await alunosContext.SaveChangesAsync();
            }
        }
    }
}

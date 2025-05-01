using MBA_DevXpert_PEO.Api.Configuration;
using MBA_DevXpert_PEO.Api.Extensions;
using MBA_DevXpert_PEO.GestaoDeAlunos.Infra.Context;
using MBA_DevXpert_PEO.GestaoDeConteudo.Application.Commands;
using MBA_DevXpert_PEO.GestaoDeConteudo.Infra.Context;
using MBA_DevXpert_PEO.PagamentoEFaturamento.Infra;
using MBA_DevXpert_PEO.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using MBA_DevXpert_PEO.PagamentoEFaturamento.Infra.Context;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Meta;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = false;
    options.ValidateOnBuild = false;
});

// Add services to the container.

builder.Services.AddControllers();
// Domínio de Conteúdo
builder.Services.AddDbContext<GestaoConteudoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Domínio de Alunos
builder.Services.AddDbContext<GestaoDeAlunosContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Domínio de Pagamentos
builder.Services.AddDbContext<PagamentosContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));



// Application-specific registrations
builder.Services.RegisterApplicationServices(); 
// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        typeof(CriarCursoCommand).Assembly,
        typeof(Program).Assembly
    ));

builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<IUser, AspNetUser>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddIdentityConfig(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerConfig();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwaggerConfig(provider);
    app.UseSwaggerUI();

    await app.UseDatabaseSeeder();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }

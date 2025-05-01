using EventSourcing;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Data.EventSourcing;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.GestaoDeAlunos.Domain.Repositories;
using MBA_DevXpert_PEO.GestaoDeAlunos.Infra.Repositories;
using MBA_DevXpert_PEO.GestaoDeConteudo.Application.Commands;
using MBA_DevXpert_PEO.GestaoDeConteudo.Application.Handlers;
using MBA_DevXpert_PEO.GestaoDeConteudo.Application.Services;
using MBA_DevXpert_PEO.GestaoDeConteudo.Domain.Repositories;
using MBA_DevXpert_PEO.GestaoDeConteudo.Infra.Repository;
using MBA_DevXpert_PEO.PagamentoEFaturamento.Infra.Repositories;
using MBA_DevXpert_PEO.PagamentoEFaturamento.Domain.Repositories;
using MediatR;

public static class ApplicationDependencyInjection
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        // Core Mediator
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        // Notification Handler
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        // Event Sourcing
        services.AddSingleton<IEventStoreService, EventStoreService>();
        services.AddSingleton<IEventSourcingRepository, EventSourcingRepository>();

        // Command Handlers
        services.AddScoped<IRequestHandler<CriarCursoCommand, Guid?>, CursoCommandHandler>();
        services.AddScoped<IRequestHandler<AdicionarAulaCommand, bool>, CursoCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateCursoCommand, bool>, CursoCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteCursoCommand, bool>, CursoCommandHandler>();

        //Services 
        services.AddScoped<ICursoAppService, CursoAppService>();
        services.AddScoped<IAlunoAppService, AlunoAppService>();

        // Repositories
        services.AddScoped<ICursoRepository, CursoRepository>();
        services.AddScoped<IAlunoRepository, AlunoRepository>();
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
    }
}
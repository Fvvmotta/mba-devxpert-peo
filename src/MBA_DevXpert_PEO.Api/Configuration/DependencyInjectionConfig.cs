using EventSourcing;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Data.EventSourcing;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.Alunos.Domain.Repositories;
using MBA_DevXpert_PEO.Alunos.Infra.Repositories;
using MBA_DevXpert_PEO.Conteudos.Application.Commands;
using MBA_DevXpert_PEO.Conteudos.Application.Handlers;
using MBA_DevXpert_PEO.Conteudos.Application.Services;
using MBA_DevXpert_PEO.Conteudos.Domain.Repositories;
using MBA_DevXpert_PEO.Conteudos.Infra.Repository;
using MBA_DevXpert_PEO.Pagamentos.Infra.Repositories;
using MBA_DevXpert_PEO.Pagamentos.Domain.Repositories;
using MediatR;
using MBA_DevXpert_PEO.Api.Identity;
using Alunos.Queries;

namespace MBA_DevXpert_PEO.Api.Configuration
{
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

            // Queries
            services.AddScoped<IAlunoQueries, AlunoQueries>();

            // Services 
            services.AddScoped<ICursoAppService, CursoAppService>();
            services.AddScoped<IAlunoAppService, AlunoAppService>();

            // Identity Services
            services.AddScoped<IUser, AspNetUser>();
            services.AddScoped<TokenService>();

            // Repositories
            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<IAlunoRepository, AlunoRepository>();
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        }
    }
}

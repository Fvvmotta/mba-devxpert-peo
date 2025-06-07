using MBA_DevXpert_PEO.Conteudos.Application.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MBA_DevXpert_PEO.Conteudos.Application.Handlers
{
    public class CursoEventHandler :
                                    INotificationHandler<CursoCriadoEvent>,
                                    INotificationHandler<AulaAdicionadaEvent>,
                                    INotificationHandler<CursoAtualizadoEvent>,
                                    INotificationHandler<CursoRemovidoEvent>,
                                    INotificationHandler<AulaRemovidaEvent>
    {
        public Task Handle(CursoCriadoEvent notification, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task Handle(AulaAdicionadaEvent notification, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task Handle(CursoAtualizadoEvent notification, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task Handle(CursoRemovidoEvent notification, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task Handle(AulaRemovidaEvent notification, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

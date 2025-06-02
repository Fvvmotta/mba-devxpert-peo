using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Alunos.Application.Events;

namespace MBA_DevXpert_PEO.Alunos.Application.Handlers
{
    public class AlunoEventHandler :
    INotificationHandler<MatriculaCriadaEvent>,
    INotificationHandler<CursoFinalizadoEvent>
    {
        public Task Handle(MatriculaCriadaEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(CursoFinalizadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}

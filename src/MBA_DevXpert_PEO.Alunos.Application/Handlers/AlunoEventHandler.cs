using MediatR;
using MBA_DevXpert_PEO.Alunos.Application.Events;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.IntegrationEvents;
using MBA_DevXpert_PEO.Alunos.Domain.Repositories;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.Core.Communication.Mediator;

namespace MBA_DevXpert_PEO.Alunos.Application.Handlers
{
    public class AlunoEventHandler :
        INotificationHandler<MatriculaCriadaEvent>,
        INotificationHandler<CursoFinalizadoEvent>,
        INotificationHandler<PedidoPagamentoRealizadoEvent>,
        INotificationHandler<PedidoPagamentoRecusadoEvent>,
        INotificationHandler<AulaFinalizadaEvent>,
        INotificationHandler<AlunoCriadoEvent>,
        INotificationHandler<AlunoAtualizadoEvent>
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public AlunoEventHandler(IAlunoRepository alunoRepository, IMediatorHandler mediatorHandler)
        {
            _alunoRepository = alunoRepository;
            _mediatorHandler = mediatorHandler;
        }
        public Task Handle(MatriculaCriadaEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public Task Handle(AulaFinalizadaEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public Task Handle(CursoFinalizadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public Task Handle(AlunoCriadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public Task Handle(AlunoAtualizadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public async Task Handle(PedidoPagamentoRealizadoEvent notification, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorId(notification.AlunoId);
            if (aluno == null) return;

            var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == notification.MatriculaId);
            if (matricula == null) return;

            if (!matricula.ConfirmarPagamento(out var erro))
            {
                // Notificar para log/observabilidade
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("Pagamento", erro));
                return;
            }

            _alunoRepository.Atualizar(aluno);
            await _alunoRepository.UnitOfWork.Commit();
        }

        public async Task Handle(PedidoPagamentoRecusadoEvent notification, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorId(notification.AlunoId);
            if (aluno == null) return;

            var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == notification.MatriculaId);
            if (matricula == null) return;

            if (!matricula.RecusarPagamento(out var erro))
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("Pagamento", erro));
                return;
            }

            _alunoRepository.Atualizar(aluno);
            await _alunoRepository.UnitOfWork.Commit();
        }
    }
}

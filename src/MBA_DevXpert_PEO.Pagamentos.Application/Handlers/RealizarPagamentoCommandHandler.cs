using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.Pagamentos.Application.Commands;
using MBA_DevXpert_PEO.Pagamentos.Application.Events;
using MBA_DevXpert_PEO.Pagamentos.Domain.Entities;
using MBA_DevXpert_PEO.Pagamentos.Domain.Repositories;
using MBA_DevXpert_PEO.Pagamentos.Domain.ValueObjects;

namespace MBA_DevXpert_PEO.GestaoDePagamentos.Application.Handlers
{
    public class RealizarPagamentoCommandHandler : 
        IRequestHandler<RealizarPagamentoCommand, bool>
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public RealizarPagamentoCommandHandler(
            IPagamentoRepository pagamentoRepository,
            IMediatorHandler mediatorHandler)
        {
            _pagamentoRepository = pagamentoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(RealizarPagamentoCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido())
            {
                await NotificarErros(command);
                return false;
            }

            var cartao = new DadosCartao(
                command.NomeTitular,
                command.NumeroCartao,
                command.Vencimento,
                command.CVV);

            var pagamento = new Pagamento(command.MatriculaId, command.Valor, cartao);

            pagamento.Confirmar(); // Simula pagamento aprovado

            _pagamentoRepository.Adicionar(pagamento);

            await _mediatorHandler.PublicarEvento(new PagamentoRealizadoEvent(pagamento.Id, pagamento.MatriculaId));

            return await _pagamentoRepository.UnitOfWork.Commit();
        }

        private async Task NotificarErros(Command<bool> command)
        {
            foreach (var error in command.ValidationResult.Errors)
            {
                await _mediatorHandler.PublicarNotificacao(
                    new DomainNotification(command.MessageType, error.ErrorMessage));
            }
        }
    }
}

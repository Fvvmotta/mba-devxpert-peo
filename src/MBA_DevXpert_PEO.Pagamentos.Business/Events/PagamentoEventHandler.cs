using MBA_DevXpert_PEO.Core.DomainObjects.DTO;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.IntegrationEvents;
using MediatR;

namespace MBA_DevXpert_PEO.Pagamentos.Business.Events
{
    public class PagamentoEventHandler : INotificationHandler<PedidoDePagamentoDeMatriculaEvent>
    {
        private readonly IPagamentoService _pagamentoService;

        public PagamentoEventHandler(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        public async Task Handle(PedidoDePagamentoDeMatriculaEvent message, CancellationToken cancellationToken)
        {
            var pagamentoPedido = new PagamentoPedido
            {
                MatriculaId = message.MatriculaId,
                AlunoId = message.AlunoId,
                Valor = message.Valor,
                NomeCartao = message.NomeCartao,
                NumeroCartao = message.NumeroCartao,
                ExpiracaoCartao = message.ExpiracaoCartao,
                CvvCartao = message.CvvCartao
            };

            await _pagamentoService.RealizarPagamentoPedido(pagamentoPedido);
        }
    }
}

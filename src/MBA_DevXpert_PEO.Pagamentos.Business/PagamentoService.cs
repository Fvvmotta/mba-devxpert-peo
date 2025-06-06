using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.DomainObjects.DTO;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.IntegrationEvents;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;

namespace MBA_DevXpert_PEO.Pagamentos.Business
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoCartaoCreditoFacade _pagamentoCartaoCreditoFacade;
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PagamentoService(IPagamentoCartaoCreditoFacade pagamentoCartaoCreditoFacade,
                                IPagamentoRepository pagamentoRepository, 
                                IMediatorHandler mediatorHandler)
        {
            _pagamentoCartaoCreditoFacade = pagamentoCartaoCreditoFacade;
            _pagamentoRepository = pagamentoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoMatricula)
        {
            var matricula = new Matricula
            {
                Id = pagamentoMatricula.MatriculaId,
                Valor = pagamentoMatricula.Valor
            };

            var pagamento = new Pagamento
            {
                MatriculaId = pagamentoMatricula.MatriculaId,
                Valor = pagamentoMatricula.Valor,
                NomeCartao = pagamentoMatricula.NomeCartao,
                NumeroCartao = pagamentoMatricula.NumeroCartao,
                ExpiracaoCartao = pagamentoMatricula.ExpiracaoCartao,
                CvvCartao = pagamentoMatricula.CvvCartao,
                Status = "Pendente"
            };

            var transacao = _pagamentoCartaoCreditoFacade.RealizarPagamento(matricula, pagamento);

            pagamento.Status = transacao.StatusTransacao == StatusTransacao.Pago ? "Pago" : "Recusado";

            if (transacao.StatusTransacao == StatusTransacao.Pago)
            {
                pagamento.AdicionarEvento(new PedidoPagamentoRealizadoEvent(
                    pagamentoMatricula.AlunoId,
                    matricula.Id,
                    transacao.PagamentoId,
                    transacao.Id,
                    matricula.Valor
                ));
                _pagamentoRepository.Adicionar(pagamento);
                _pagamentoRepository.AdicionarTransacao(transacao);
                await _pagamentoRepository.UnitOfWork.Commit();
            }
            else
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pagamento", "A operadora recusou o pagamento"));

                await _mediatorHandler.PublicarEvento(new PedidoPagamentoRecusadoEvent(
                    pagamentoMatricula.AlunoId,
                    matricula.Id,
                    transacao.PagamentoId,
                    transacao.Id,
                    matricula.Valor
                ));
            }

            return transacao;
        }

    }
}

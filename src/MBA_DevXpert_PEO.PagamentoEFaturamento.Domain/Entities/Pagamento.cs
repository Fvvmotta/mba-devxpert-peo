using MBA_DevXpert_PEO.Core.DomainObjects;
using MBA_DevXpert_PEO.PagamentoEFaturamento.Domain.ValueObjects;

namespace MBA_DevXpert_PEO.PagamentoEFaturamento.Domain.Entities
{
    public class Pagamento : Entity, IAggregateRoot
    {
        public Guid MatriculaId { get; private set; }
        public decimal Valor { get; private set; }
        public DadosCartao Cartao { get; private set; }
        public StatusPagamento Status { get; private set; }
        public DateTime DataPagamento { get; private set; }

        protected Pagamento() { }

        public Pagamento(Guid matriculaId, decimal valor, DadosCartao cartao)
        {
            Validacoes.ValidarSeMenorQue(valor, 0, "Valor do pagamento deve ser maior que zero.");
            Validacoes.ValidarSeNulo(cartao, "Dados do cartão são obrigatórios.");

            MatriculaId = matriculaId;
            Valor = valor;
            Cartao = cartao;
            Status = StatusPagamento.Pendente;
            DataPagamento = DateTime.UtcNow;
        }

        public void Confirmar()
        {
            Status = StatusPagamento.Aprovado;
            // RaiseEvent(new PagamentoRealizadoEvent(...));
        }

        public void Rejeitar()
        {
            Status = StatusPagamento.Recusado;
            // RaiseEvent(new PagamentoRecusadoEvent(...));
        }
    }
}

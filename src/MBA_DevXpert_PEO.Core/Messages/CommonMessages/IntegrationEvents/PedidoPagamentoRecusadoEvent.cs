namespace MBA_DevXpert_PEO.Core.Messages.CommonMessages.IntegrationEvents
{
    public class PedidoPagamentoRecusadoEvent : IntegrationEvent
    {
        public Guid AlunoId { get; private set; }
        public Guid MatriculaId { get; private set; }
        public Guid PagamentoId { get; private set; }
        public Guid TransacaoId { get; private set; }
        public decimal Valor { get; private set; }

        public PedidoPagamentoRecusadoEvent(Guid alunoId, Guid matriculaId, Guid pagamentoId, Guid transacaoId, decimal valor)
        {
            AggregateId = alunoId; 
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            PagamentoId = pagamentoId;
            TransacaoId = transacaoId;
            Valor = valor;
        }
    }
}

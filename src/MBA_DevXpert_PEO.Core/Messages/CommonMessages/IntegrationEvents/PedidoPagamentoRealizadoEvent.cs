namespace MBA_DevXpert_PEO.Core.Messages.CommonMessages.IntegrationEvents
{
    public class PedidoPagamentoRealizadoEvent : IntegrationEvent
    {
        public Guid AlunoId { get; private set; }
        public Guid MatriculaId { get; private set; }
        public Guid PagamentoId { get; private set; }
        public Guid TransacaoId { get; private set; }
        public decimal Valor { get; private set; }

        public PedidoPagamentoRealizadoEvent(Guid alunoId, Guid matriculaId, Guid pagamentoId, Guid transacaoId, decimal valor)
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

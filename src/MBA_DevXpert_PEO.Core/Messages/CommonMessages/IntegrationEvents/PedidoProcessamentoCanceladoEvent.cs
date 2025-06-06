using MBA_DevXpert_PEO.Core.DomainObjects.DTO;

namespace MBA_DevXpert_PEO.Core.Messages.CommonMessages.IntegrationEvents
{
    public class PedidoProcessamentoCanceladoEvent : IntegrationEvent
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }
        public CursoPedido CursoPedido { get; private set; }

        public PedidoProcessamentoCanceladoEvent(Guid pedidoId, Guid clienteId, CursoPedido cursoPedido)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
            CursoPedido = cursoPedido;
        }
    }
}

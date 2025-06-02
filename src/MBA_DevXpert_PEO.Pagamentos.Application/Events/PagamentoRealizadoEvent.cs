using MBA_DevXpert_PEO.Core.Messages;
using System;

namespace MBA_DevXpert_PEO.Pagamentos.Application.Events
{
    public class PagamentoRealizadoEvent : Event
    {
        public Guid PagamentoId { get; }
        public Guid MatriculaId { get; }

        public PagamentoRealizadoEvent(Guid pagamentoId, Guid matriculaId)
        {
            AggregateId = pagamentoId;
            PagamentoId = pagamentoId;
            MatriculaId = matriculaId;
        }
    }
}

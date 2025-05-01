using MBA_DevXpert_PEO.Core.Data.EventSourcing;
using MBA_DevXpert_PEO.Core.Messages;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.DomainEvents;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MediatR;

namespace MBA_DevXpert_PEO.Core.Communication.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventSourcingRepository _eventSourcingRepository;

        public MediatorHandler(IMediator mediator, IEventSourcingRepository eventSourcingRepository)
        {
            _mediator = mediator;
            _eventSourcingRepository = eventSourcingRepository;
        }

        public async Task<TResponse> EnviarComando<TResponse>(Command<TResponse> comando)
        {
            return await _mediator.Send(comando);
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
            _eventSourcingRepository.SalvarEvento(evento);
        }

        public async Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification
        {
            await _mediator.Publish(notificacao);
        }

        public async Task PublicarDomainEvent<T>(T notificacao) where T : DomainEvent
        {
            await _mediator.Publish(notificacao);
        }
    }
}

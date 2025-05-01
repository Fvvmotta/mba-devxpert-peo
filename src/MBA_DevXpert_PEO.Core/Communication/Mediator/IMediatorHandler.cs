using MBA_DevXpert_PEO.Core.Messages;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.DomainEvents;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;

namespace MBA_DevXpert_PEO.Core.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<TResponse> EnviarComando<TResponse>(Command<TResponse> comando);
        Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;
        Task PublicarDomainEvent<T>(T notificacao) where T : DomainEvent;
    }
}

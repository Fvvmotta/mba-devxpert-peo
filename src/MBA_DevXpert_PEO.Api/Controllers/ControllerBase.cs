using Microsoft.AspNetCore.Mvc;
using MediatR;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using System.Security.Claims;

namespace MBA_DevXpert_PEO.Api.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected readonly DomainNotificationHandler _notifications;
        protected readonly IMediatorHandler _mediatorHandler;

        protected Guid UsuarioId
        {
            get
            {
                var userId = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                return Guid.TryParse(userId, out var id) ? id : Guid.Empty;
            }
        }

        protected ControllerBase(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediatorHandler = mediatorHandler;
        }

        protected bool OperacaoValida()
        {
            return !_notifications.TemNotificacao();
        }

        protected IEnumerable<string> ObterMensagensErro()
        {
            return _notifications.ObterNotificacoes().Select(n => n.Value).ToList();
        }

        protected void NotificarErro(string codigo, string mensagem)
        {
            _mediatorHandler.PublicarNotificacao(new DomainNotification(codigo, mensagem));
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
                return Ok(result);

            return BadRequest(new
            {
                sucesso = false,
                erros = ObterMensagensErro()
            });
        }
    }
}

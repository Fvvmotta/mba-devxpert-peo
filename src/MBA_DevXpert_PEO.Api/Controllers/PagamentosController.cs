using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;

namespace MBA_DevXpert_PEO.Api.Controllers
{
    [Authorize(Roles = "Aluno")]
    [ApiController]
    [Route("api/pagamentos")]
    public class PagamentosController : BaseController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public PagamentosController(IMediatorHandler mediatorHandler,
                                    INotificationHandler<DomainNotification> notifications)
            : base(notifications, mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }
    }
}

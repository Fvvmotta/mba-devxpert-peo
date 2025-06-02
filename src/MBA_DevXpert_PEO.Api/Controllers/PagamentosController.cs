using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MBA_DevXpert_PEO.Pagamentos.Application.Commands;
using MBA_DevXpert_PEO.Pagamentos.Application.DTOs;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;

namespace MBA_DevXpert_PEO.Api.Controllers
{
    [Authorize(Roles = "Aluno")]
    [ApiController]
    [Route("api/pagamentos")]
    public class PagamentosController : ControllerBase
    {
        private readonly IMediatorHandler _mediatorHandler;

        public PagamentosController(IMediatorHandler mediatorHandler,
                                    INotificationHandler<DomainNotification> notifications)
            : base(notifications, mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpPost]
        public async Task<IActionResult> RealizarPagamento(RealizarPagamentoDto dto)
        {
            var command = new RealizarPagamentoCommand
            {
                MatriculaId = dto.MatriculaId,
                Valor = dto.Valor,
                NomeTitular = dto.NomeTitular,
                NumeroCartao = dto.NumeroCartao,
                Vencimento = dto.Vencimento,
                CVV = dto.CVV
            };

            var sucesso = await _mediatorHandler.EnviarComando(command);

            if (!sucesso)
                return BadRequest("Não foi possível processar o pagamento.");

            return Ok(new { message = "Pagamento realizado com sucesso." });
        }
    }
}

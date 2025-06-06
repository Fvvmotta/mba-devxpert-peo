

using Microsoft.AspNetCore.Mvc;
using MediatR;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.IntegrationEvents;
using MBA_DevXpert_PEO.Core.DomainObjects.DTO;
using MBA_DevXpert_PEO.Conteudos.Application.Queries;

namespace MBA_DevXpert_PEO.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursoController : BaseController
    {
        private readonly ICursoQueries _cursoQueries;

        public CursoController( INotificationHandler<DomainNotification> notifications,
                                ICursoQueries cursoQueries,
                                IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            _cursoQueries = cursoQueries;
        }

        [HttpGet]
        public async Task<IActionResult> ObterCursos()
        {
            var cursos = await _cursoQueries.ObterTodos();
            return CustomResponse(cursos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterCursoPorId(Guid id)
        {
            var curso = await _cursoQueries.ObterPorId(id);

            if (curso == null)
            {
                NotificarErro("Curso", "Curso não encontrado.");
                return CustomResponse();
            }

            return CustomResponse(curso);
        }
    }
}

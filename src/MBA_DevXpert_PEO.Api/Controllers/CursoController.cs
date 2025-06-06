

using Microsoft.AspNetCore.Mvc;
using MediatR;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.Conteudos.Application.Services;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.IntegrationEvents;
using MBA_DevXpert_PEO.Core.DomainObjects.DTO;

namespace MBA_DevXpert_PEO.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursoController : BaseController
    {
        private readonly ICursoAppService _cursoAppService;

        public CursoController(ICursoAppService cursoAppService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            _cursoAppService = cursoAppService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterCursos()
        {
            var cursos = await _cursoAppService.ObterTodos();
            return CustomResponse(cursos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterCursoPorId(Guid id)
        {
            var curso = await _cursoAppService.ObterPorId(id);

            if (curso == null)
            {
                NotificarErro("Curso", "Curso não encontrado.");
                return CustomResponse();
            }

            return CustomResponse(curso);
        }
    }
}

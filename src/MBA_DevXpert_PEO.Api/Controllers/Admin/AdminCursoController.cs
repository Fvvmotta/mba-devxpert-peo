using Microsoft.AspNetCore.Mvc;
using MediatR;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.Conteudos.Application.DTOs;
using MBA_DevXpert_PEO.Conteudos.Application.Commands;
using MBA_DevXpert_PEO.Conteudos.Application.Services;

namespace MBA_DevXpert_PEO.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminCursoController : BaseController
    {
        private readonly ICursoAppService _cursoAppService;

        public AdminCursoController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler,
            ICursoAppService cursoAppService)
            : base(notifications, mediatorHandler)
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

        [HttpPost]
        public async Task<IActionResult> CriarCurso([FromBody] CursoInputDTO dto)
        {
            if (!ModelState.IsValid)
            {
                NotificarErro("Curso", "Dados inválidos.");
                return CustomResponse();
            }

            var command = new CriarCursoCommand(dto.Nome, dto.Autor, dto.CargaHoraria, dto.DescricaoConteudoProgramatico);

            var cursoId = await _mediatorHandler.EnviarComando(command);

            if (cursoId == null)
            {
                NotificarErro("Curso", "Erro ao criar o curso.");
                return CustomResponse();
            }

            return CustomResponse(new { Id = cursoId });
        }

        [HttpPost("{cursoId}/aulas")]
        public async Task<IActionResult> AdicionarAula(Guid cursoId, [FromBody] AdicionarAulaInputDTO dto)
        {
            if (cursoId != dto.CursoId)
            {
                NotificarErro("Aula", "ID do curso na URL não bate com o corpo da requisição.");
                return CustomResponse();
            }

            if (!ModelState.IsValid)
            {
                NotificarErro("Aula", "Dados inválidos.");
                return CustomResponse();
            }

            var command = new AdicionarAulaCommand(dto.CursoId, dto.Titulo, dto.MaterialUrl);

            var sucesso = await _mediatorHandler.EnviarComando(command);

            if (!sucesso)
            {
                NotificarErro("Aula", "Erro ao adicionar aula.");
                return CustomResponse();
            }

            return CustomResponse("Aula adicionada com sucesso.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarCurso(Guid id, [FromBody] UpdateCursoInputDTO dto)
        {
            if (id != dto.Id)
            {
                NotificarErro("Curso", "Id do curso na URL não corresponde ao corpo.");
                return CustomResponse();
            }

            if (!ModelState.IsValid)
            {
                NotificarErro("Curso", "Dados inválidos.");
                return CustomResponse();
            }

            var command = new UpdateCursoCommand(dto.Id, dto.Nome, dto.Autor, dto.CargaHoraria, dto.DescricaoConteudoProgramatico);

            var sucesso = await _mediatorHandler.EnviarComando(command);

            if (!sucesso)
            {
                NotificarErro("Curso", "Erro ao atualizar o curso.");
                return CustomResponse();
            }

            return CustomResponse("Curso atualizado com sucesso.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarCurso(Guid id)
        {
            var sucesso = await _mediatorHandler.EnviarComando(new DeleteCursoCommand(id));

            if (!sucesso)
            {
                NotificarErro("Curso", "Erro ao deletar o curso.");
                return CustomResponse();
            }

            return CustomResponse("Curso deletado com sucesso.");
        }

        [HttpDelete("{cursoId}/aulas/{aulaId}")]
        public async Task<IActionResult> DeletarAula(Guid cursoId, Guid aulaId)
        {
            var comando = new DeleteAulaCursoCommand(cursoId, aulaId);
            var sucesso = await _mediatorHandler.EnviarComando(comando);

            if (!sucesso)
            {
                NotificarErro("Aula", "Erro ao deletar aula.");
                return CustomResponse();
            }

            return CustomResponse("Aula deletada com sucesso.");
        }

    }
}

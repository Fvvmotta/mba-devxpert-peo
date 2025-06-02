using MediatR;
using Microsoft.AspNetCore.Mvc;
using MBA_DevXpert_PEO.Alunos.Application.Commands;
using MBA_DevXpert_PEO.Alunos.Application.DTOs;
using MBA_DevXpert_PEO.Conteudos.Application.Services;

namespace MBA_DevXpert_PEO.Alunos.API.Controllers
{
    [ApiController]
    [Route("api/alunos")]
    public class AlunoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAlunoAppService _alunoAppService;

        public AlunoController(IAlunoAppService alunoAppService, IMediator mediator)
        {
            _alunoAppService = alunoAppService;
            _mediator = mediator;
        }

        [HttpPost("{alunoId}/matriculas")]
        public async Task<IActionResult> CriarMatricula(Guid alunoId, CriarMatriculaDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var command = new CriarMatriculaCommand(alunoId, dto.CursoId);
            var resultado = await _mediator.Send(command);

            if (!resultado)
                return BadRequest("Não foi possível criar a matrícula.");

            return Ok("Matrícula criada com sucesso.");
        }

        [HttpPost("{alunoId}/matriculas/{matriculaId}/finalizar")]
        public async Task<IActionResult> FinalizarCurso(Guid alunoId, Guid matriculaId)
        {
            var command = new FinalizarCursoCommand(alunoId, matriculaId);
            var resultado = await _mediator.Send(command);

            if (!resultado)
                return BadRequest("Não foi possível finalizar o curso.");

            return Ok("Curso finalizado e certificado gerado.");
        }
        [HttpPut("{alunoId}")]
        public async Task<IActionResult> AtualizarAluno(Guid alunoId, AtualizarAlunoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var command = new AtualizarAlunoCommand(alunoId, dto.Nome, dto.Email);
            var sucesso = await _mediator.Send(command);

            if (!sucesso) return BadRequest("Falha ao atualizar dados.");
            return Ok("Dados atualizados com sucesso.");
        }
        [HttpGet("matriculas-detalhadas")]
        public async Task<IActionResult> ObterMatriculasDetalhadas()
        {
            var resultado = await _alunoAppService.ObterMatriculasDetalhadas();
            return Ok(resultado);
        }
    }
}

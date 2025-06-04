using MediatR;
using Microsoft.AspNetCore.Mvc;
using MBA_DevXpert_PEO.Alunos.Application.Commands;
using MBA_DevXpert_PEO.Alunos.Application.DTOs;
using MBA_DevXpert_PEO.Conteudos.Application.Services;
using Alunos.Queries;

namespace MBA_DevXpert_PEO.Alunos.API.Controllers
{
    [ApiController]
    [Route("api/alunos")]
    public class AlunoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAlunoAppService _alunoAppService;
        private readonly IAlunoQueries _alunoQueries;
        private readonly ICursoAppService _cursoAppService;

        public AlunoController(IAlunoAppService alunoAppService, 
                                IAlunoQueries alunoQueries,
                                ICursoAppService cursoAppService,
                                IMediator mediator)
        {
            _alunoAppService = alunoAppService;
            _alunoQueries = alunoQueries;
            _cursoAppService = cursoAppService;
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
            var matricula = await _alunoQueries.ObterMatriculaComCurso(alunoId, matriculaId);
            if (matricula == null)
                return BadRequest("Matrícula não encontrada.");

            var curso = await _cursoAppService.ObterPorCursoId(matricula.CursoId);
            if (curso == null)
                return BadRequest("Curso não encontrado.");
            var aluno = await _alunoQueries.ObterPorId(alunoId);
            if (aluno == null)
                return BadRequest("Aluno não encontrado.");

            var command = new FinalizarCursoCommand(
                alunoId,
                matriculaId,
                aluno.Nome,
                curso.Nome,
                curso.CargaHoraria
            );
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
            var resultado = await _alunoAppService.ObterAlunosComMatriculas();
            return Ok(resultado);
        }
        [HttpGet("{alunoId}/matriculas/{matriculaId}/emitir-certificado")]
        public async Task<IActionResult> EmitirCertificado(Guid alunoId, Guid matriculaId)
        {
            var certificado = await _alunoQueries.ObterCertificado(alunoId, matriculaId);

            if (certificado == null)
                return NotFound("Certificado não encontrado.");

            return Ok(certificado);
        }
    }
}

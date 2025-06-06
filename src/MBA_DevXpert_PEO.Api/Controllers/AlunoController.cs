using MediatR;
using Microsoft.AspNetCore.Mvc;
using MBA_DevXpert_PEO.Alunos.Application.Commands;
using MBA_DevXpert_PEO.Alunos.Application.DTOs;
using Alunos.Queries;
using MBA_DevXpert_PEO.Core.DomainObjects.DTO;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.IntegrationEvents;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Api.Controllers;
using Alunos.Commands;
using MBA_DevXpert_PEO.Conteudos.Application.Queries;
using MBA_DevXpert_PEO.Api.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MBA_DevXpert_PEO.Alunos.API.Controllers
{
    [ApiController]
    [Route("api/alunos")]
    public class AlunoController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAlunoQueries _alunoQueries;
        private readonly ICursoQueries _cursoQueries;

        public AlunoController(IAlunoQueries alunoQueries,
                                ICursoQueries cursoQueries,
                                INotificationHandler<DomainNotification> notifications,
                                UserManager<ApplicationUser> userManager,
                                IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            _alunoQueries = alunoQueries;
            _cursoQueries = cursoQueries;
            _userManager = userManager;
        }

        [HttpPost("{alunoId}/matriculas")]
        public async Task<IActionResult> CriarMatricula(Guid alunoId, CriarMatriculaDto dto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var curso = await _cursoQueries.ObterResumoCurso(dto.CursoId);
            if (curso == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("Curso", "Curso não encontrado."));
                return CustomResponse();
            }

            var command = new CriarMatriculaCommand(alunoId, dto.CursoId, curso.Valor, curso.TotalAulas);
            var resultado = await _mediatorHandler.EnviarComando(command);

            return CustomResponse(resultado ? "Matrícula criada com sucesso." : null);
        }

        [HttpPost("{matriculaId}/pagar")]
        public async Task<IActionResult> PagarMatricula(Guid matriculaId, [FromBody] PagamentoPedido dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var matricula = await _alunoQueries.ObterMatriculaPorId(matriculaId);
            if (matricula == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("Matrícula", "Matrícula não encontrada."));
                return CustomResponse();
            }

            var curso = await _cursoQueries.ObterPorCursoId(matricula.CursoId);
            if (curso == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("Curso", "Curso não encontrado."));
                return CustomResponse();
            }

            var valor = curso;

            // Publicar evento
            var evento = new PedidoDePagamentoDeMatriculaEvent(
                matriculaId,
                dto.AlunoId,
                matricula.Valor,
                dto.NomeCartao,
                dto.NumeroCartao,
                dto.ExpiracaoCartao,
                dto.CvvCartao
            );

            await _mediatorHandler.PublicarEvento(evento);

            return CustomResponse("Pedido de pagamento enviado com sucesso.");
        }


        [HttpPost("{alunoId}/matricula/{matriculaId}/aula/finalizar")]
        public async Task<IActionResult> FinalizarAula(Guid alunoId, Guid matriculaId)
        {
            var matricula = await _alunoQueries.ObterMatriculaPorId(matriculaId);
            if (matricula == null) return NotFound();

            var cursoInfo = await _cursoQueries.ObterResumoCurso(matricula.CursoId);
            if (cursoInfo == null) return NotFound();

            var totalAulas = cursoInfo.TotalAulas;

            var sucesso = await _mediatorHandler.EnviarComando(new FinalizarAulaCommand(alunoId, matriculaId, totalAulas));
            if (!sucesso) return BadRequest("Não foi possível finalizar a aula.");

            return Ok("Aula finalizada com sucesso.");
        }
        
        [HttpGet("progresso")]
        public async Task<IActionResult> ObterProgresso([FromQuery] Guid matriculaId, [FromQuery] Guid cursoId)
        {
            var aluno = await _alunoQueries.ObterPorId(UsuarioId);
            if (aluno == null)
                return NotFound("Aluno não encontrado.");

            var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == matriculaId && m.CursoId == cursoId);
            if (matricula == null)
                return NotFound("Matrícula não encontrada para este curso.");

            var historico = matricula.Historico;

            var resultado = new
            {
                MatriculaId = matricula.Id,
                CursoId = cursoId,
                TotalAulas = historico.TotalAulas,
                AulasConcluidas = historico.AulasConcluidas,
                PorcentagemConcluida = historico.TotalAulas == 0 ? 0 : Math.Round((double)historico.AulasConcluidas / historico.TotalAulas * 100, 2),
                TodasAulasConcluidas = historico.TodasAulasConcluidas
            };

            return Ok(resultado);
        }

        [HttpPost("{alunoId}/matriculas/{matriculaId}/finalizar")]
        public async Task<IActionResult> FinalizarCurso(Guid alunoId, Guid matriculaId)
        {
            var matricula = await _alunoQueries.ObterMatriculaComCurso(alunoId, matriculaId);
            if (matricula == null)
                return BadRequest("Matrícula não encontrada.");

            var curso = await _cursoQueries.ObterPorCursoId(matricula.CursoId);
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
            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
            {
                var domainNotifications = ((DomainNotificationHandler)_notifications).ObterNotificacoes();
                var mensagens = domainNotifications.Select(n => n.Value).ToList();
                return BadRequest(new { erros = mensagens });
            }

            return Ok("Curso finalizado e certificado gerado.");
        }

        [HttpGet("{alunoId}/matriculas/{matriculaId}/emitir-certificado")]
        public async Task<IActionResult> EmitirCertificado(Guid alunoId, Guid matriculaId)
        {
            var certificado = await _alunoQueries.ObterCertificado(alunoId, matriculaId);

            if (certificado == null)
                return NotFound("Certificado não encontrado.");

            return Ok(certificado);
        }

        [HttpGet("perfil")]
        public async Task<IActionResult> ObterPerfil()
        {
            var aluno = await _alunoQueries.ObterPorId(UsuarioId);
            if (aluno == null)
                return NotFound("Aluno não encontrado.");

            var response = new
            {
                aluno.Id,
                aluno.Nome,
                aluno.Email
            };

            return Ok(response);
        }


        [HttpPut("perfil")]
        public async Task<IActionResult> AtualizarPerfil(AtualizarAlunoIdentityDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuarioId = User.GetUserId(); 
            if (UsuarioId != dto.Id) return Unauthorized("Acesso negado.");

            var command = new AtualizarAlunoCommand(dto.Id, dto.Nome, dto.Email);
            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
            {
                var erros = ((DomainNotificationHandler)_notifications).ObterNotificacoes().Select(n => n.Value);
                return BadRequest(new { erros });
            }

            var identityUser = await _userManager.FindByIdAsync(dto.Id.ToString());
            if (identityUser == null) return NotFound("Usuário não encontrado.");

            identityUser.Email = dto.Email;
            identityUser.UserName = dto.Email;

            var identityResult = await _userManager.UpdateAsync(identityUser);
            if (!identityResult.Succeeded)
                return BadRequest("Erro ao atualizar usuário.");

            if (!string.IsNullOrWhiteSpace(dto.NovaSenha))
            {
                if (dto.NovaSenha != dto.ConfirmarSenha)
                    return BadRequest("As senhas não conferem.");

                var remove = await _userManager.RemovePasswordAsync(identityUser);
                if (!remove.Succeeded) return BadRequest("Erro ao remover senha antiga.");

                var add = await _userManager.AddPasswordAsync(identityUser, dto.NovaSenha);
                if (!add.Succeeded) return BadRequest("Erro ao definir nova senha.");
            }

            return Ok("Perfil atualizado com sucesso.");
        }
    }
}

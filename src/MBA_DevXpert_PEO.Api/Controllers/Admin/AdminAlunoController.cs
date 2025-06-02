using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using MBA_DevXpert_PEO.Alunos.Domain.Repositories;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.Conteudos.Application.Services;
using MediatR;
using MBA_DevXpert_PEO.Alunos.Application.DTOs;

namespace MBA_DevXpert_PEO.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/adminaluno")]
    public class AdminAlunoController : ControllerBase
    {
        private readonly IAlunoRepository _alunoRepository;
        public AdminAlunoController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler,
            IAlunoRepository alunoRepository)
            : base(notifications, mediatorHandler)
        {
            _alunoRepository = alunoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var alunos = await _alunoRepository.ObterTodos();
            return Ok(alunos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var aluno = await _alunoRepository.ObterPorId(id);
            return aluno is null ? NotFound() : Ok(aluno);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CriarAlunoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var aluno = new Aluno(dto.Nome, dto.Email);
            _alunoRepository.Adicionar(aluno);
            await _alunoRepository.UnitOfWork.Commit();

            return CreatedAtAction(nameof(ObterPorId), new { id = aluno.Id }, aluno);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, AtualizarAlunoDto dto)
        {
            if (id != dto.Id) return BadRequest("IDs não coincidem.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var aluno = await _alunoRepository.ObterPorId(id);
            if (aluno == null) return NotFound();

            aluno.AtualizarDados(dto.Nome, dto.Email);
            _alunoRepository.Atualizar(aluno);
            await _alunoRepository.UnitOfWork.Commit();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var aluno = await _alunoRepository.ObterPorId(id);
            if (aluno == null) return NotFound();

            _alunoRepository.Remover(aluno);
            await _alunoRepository.UnitOfWork.Commit();

            return NoContent();
        }
    }
}

using Alunos.Queries;
using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using MBA_DevXpert_PEO.Alunos.Domain.Repositories;
using MBA_DevXpert_PEO.Api.Controllers;
using MBA_DevXpert_PEO.Api.DTOs;
using MBA_DevXpert_PEO.Api.Identity;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/adminaluno")]
public class AdminAlunoController : BaseController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAlunoRepository _alunoRepository;
    private readonly IAlunoQueries _alunoQueries;

    public AdminAlunoController(
        IAlunoQueries alunoQueries,
        UserManager<ApplicationUser> userManager,
        INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediatorHandler,
        IAlunoRepository alunoRepository)
        : base(notifications, mediatorHandler)
    {
        _alunoQueries = alunoQueries;
        _userManager = userManager;
        _alunoRepository = alunoRepository;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var alunos = await _alunoQueries.ObterAlunosComMatriculas();
        return Ok(alunos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var aluno = await _alunoQueries.ObterPorId(id);
        return aluno is null ? NotFound() : Ok(aluno);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(UserRegisterDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var alunoId = Guid.NewGuid();
        var identityUser = new ApplicationUser
        {
            Id = alunoId,
            UserName = dto.Email,
            Email = dto.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(identityUser, dto.Senha);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return BadRequest(ModelState);
        }

        var aluno = Aluno.CriarComId(alunoId, dto.Nome, dto.Email);
        _alunoRepository.Adicionar(aluno);
        await _alunoRepository.UnitOfWork.Commit();

        return CustomResponse("Aluno criado com sucesso.");
    }

    [HttpGet("matriculas-detalhadas")]
    public async Task<IActionResult> ObterMatriculasDetalhadas()
    {
        var resultado = await _alunoQueries.ObterAlunosComMatriculas();
        return Ok(resultado);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, AtualizarAlunoIdentityDto dto)
    {
        if (id != dto.Id) return BadRequest("IDs não coincidem.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var command = new AtualizarAlunoCommand(dto.Id, dto.Nome, dto.Email);
        var resultado = await _mediatorHandler.EnviarComando(command);

        if (!resultado)
            return BadRequest("Erro ao atualizar dados do aluno.");

        var identityUser = await _userManager.FindByIdAsync(dto.Id.ToString());
        if (identityUser == null) return NotFound("Usuário Identity não encontrado.");

        identityUser.Email = dto.Email;
        identityUser.UserName = dto.Email;

        var identityResult = await _userManager.UpdateAsync(identityUser);
        if (!identityResult.Succeeded)
            return BadRequest("Erro ao atualizar usuário Identity.");

        if (!string.IsNullOrWhiteSpace(dto.NovaSenha))
        {
            if (dto.NovaSenha != dto.ConfirmarSenha)
                return BadRequest("As senhas não conferem.");

            var remove = await _userManager.RemovePasswordAsync(identityUser);
            if (!remove.Succeeded) return BadRequest("Erro ao remover senha antiga.");

            var add = await _userManager.AddPasswordAsync(identityUser, dto.NovaSenha);
            if (!add.Succeeded) return BadRequest("Erro ao definir nova senha.");
        }

        return CustomResponse("Aluno atualizado com sucesso.");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        var aluno = await _alunoRepository.ObterPorId(id);
        if (aluno == null) return NotFound();

        _alunoRepository.Remover(aluno);
        await _alunoRepository.UnitOfWork.Commit();

        return CustomResponse("Aluno removido com sucesso.");
    }
}

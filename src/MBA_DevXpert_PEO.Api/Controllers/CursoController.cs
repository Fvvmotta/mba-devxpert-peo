

using Microsoft.AspNetCore.Mvc;
using MediatR;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using Commands = MBA_DevXpert_PEO.GestaoDeConteudo.Application.Commands;
using MBA_DevXpert_PEO.GestaoDeConteudo.Application.Handlers;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.GestaoDeConteudo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MBA_DevXpert_PEO.GestaoDeConteudo.Application.DTOs;
using MBA_DevXpert_PEO.GestaoDeConteudo.Domain.Repositories;
using MBA_DevXpert_PEO.GestaoDeConteudo.Application.Services;
using MBA_DevXpert_PEO.GestaoDeConteudo.Application.Commands;

namespace MBA_DevXpert_PEO.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursoController : ControllerBase
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

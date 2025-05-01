using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using FluentValidation.Results;
using FluentValidation;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Data;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.GestaoDeConteudo.Application.Commands;
using MBA_DevXpert_PEO.GestaoDeConteudo.Application.Handlers;
using MBA_DevXpert_PEO.GestaoDeConteudo.Domain.Entities;
using MBA_DevXpert_PEO.GestaoDeConteudo.Domain.Repositories;
using MBA_DevXpert_PEO.GestaoDeConteudo.Domain.ValueObjects;

namespace MBA_DevXpert_PEO.Tests.Unit.GestaoDeConteudo.Handlers
{
    public class CursoCommandHandlerTests
    {
        [Fact]
        public async Task Handle_CriarCurso_DeveCriarCurso_QuandoCommandEhValido()
        {
            var cursoRepositoryMock = new Mock<ICursoRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
            cursoRepositoryMock.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);
            var mediatorMock = new Mock<IMediatorHandler>();

            var handler = new CursoCommandHandler(cursoRepositoryMock.Object, mediatorMock.Object);

            var command = new CriarCursoCommand("Curso Teste", "Autor Teste", 40, "Conteudo válido");
            command.ValidationResult = new ValidationResult();

            var resultado = await handler.Handle(command, CancellationToken.None);

            resultado.Should().NotBeNull();
            cursoRepositoryMock.Verify(r => r.Adicionar(It.IsAny<Curso>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_CriarCurso_DeveRetornarNull_QuandoCommandEhInvalido()
        {
            var cursoRepositoryMock = new Mock<ICursoRepository>();
            var mediatorMock = new Mock<IMediatorHandler>();

            var handler = new CursoCommandHandler(cursoRepositoryMock.Object, mediatorMock.Object);

            var command = new CriarCursoCommand("", "", 0, "");
            command.ValidationResult = new ValidationResult(new List<ValidationFailure>
            {
                new("Nome", "Nome inválido"),
                new("Autor", "Autor inválido"),
                new("CargaHoraria", "Carga horária inválida"),
                new("Descricao", "Descrição inválida")
            });

            var resultado = await handler.Handle(command, CancellationToken.None);

            resultado.Should().BeNull();
            cursoRepositoryMock.Verify(r => r.Adicionar(It.IsAny<Curso>()), Times.Never);
        }

        [Fact]
        public async Task Handle_AdicionarAula_DeveRetornarFalse_QuandoCommandEhInvalido()
        {
            var cursoRepositoryMock = new Mock<ICursoRepository>();
            var mediatorMock = new Mock<IMediatorHandler>();

            var handler = new CursoCommandHandler(cursoRepositoryMock.Object, mediatorMock.Object);

            var command = new AdicionarAulaCommand(Guid.Empty, "", "");
            command.ValidationResult = new ValidationResult(new List<ValidationFailure>
            {
                new("CursoId", "Curso inválido")
            });

            var resultado = await handler.Handle(command, CancellationToken.None);

            resultado.Should().BeFalse();
            cursoRepositoryMock.Verify(r => r.ObterPorId(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Handle_AdicionarAula_DeveAdicionarAula_QuandoCommandEhValido()
        {
            var curso = new Curso("Curso Teste", "Fernando Motta", 20, ConteudoProgramatico.Criar("Conteúdo Teste"));
            var cursoRepositoryMock = new Mock<ICursoRepository>();
            cursoRepositoryMock.Setup(r => r.ObterPorId(It.IsAny<Guid>())).ReturnsAsync(curso);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
            cursoRepositoryMock.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

            var mediatorMock = new Mock<IMediatorHandler>();
            var handler = new CursoCommandHandler(cursoRepositoryMock.Object, mediatorMock.Object);

            var command = new AdicionarAulaCommand(Guid.NewGuid(), "Título Aula", "Descrição Aula", "https://material.com/aula");
            command.ValidationResult = new ValidationResult();

            var resultado = await handler.Handle(command, CancellationToken.None);

            resultado.Should().BeTrue();
            curso.Aulas.Should().HaveCount(1);
            cursoRepositoryMock.Verify(r => r.Atualizar(It.IsAny<Curso>()), Times.Once);
        }
    }
}

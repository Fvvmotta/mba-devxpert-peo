using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.GestaoDeAlunos.Application.Commands;
using MBA_DevXpert_PEO.GestaoDeAlunos.Application.Handlers;
using MBA_DevXpert_PEO.GestaoDeAlunos.Domain.Repositories;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.GestaoDeAlunos.Domain.Entities;
using MBA_DevXpert_PEO.GestaoDeAlunos.Domain.Entities.Enum;
using MBA_DevXpert_PEO.GestaoDeAlunos.Application.Events;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;

namespace MBA_DevXpert_PEO.Tests.Unit.GestaoDeAlunos.Application
{
    public class AlunoCommandHandlerTests
    {
        private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly AlunoCommandHandler _handler;

        public AlunoCommandHandlerTests()
        {
            _alunoRepositoryMock = new Mock<IAlunoRepository>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _handler = new AlunoCommandHandler(_alunoRepositoryMock.Object, _mediatorHandlerMock.Object);
        }

        [Fact]
        public async Task CriarMatricula_DeveAdicionarMatriculaEPublicarEvento()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aluno = new Aluno("João", "joao@email.com");

            _alunoRepositoryMock.Setup(r => r.ObterPorId(alunoId))
                .ReturnsAsync(aluno);
            _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(true);

            var command = new CriarMatriculaCommand(alunoId, cursoId);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(resultado);
            _alunoRepositoryMock.Verify(r => r.Atualizar(It.IsAny<Aluno>()), Times.Once);
            _mediatorHandlerMock.Verify(m => m.PublicarEvento(It.IsAny<CursoFinalizadoEvent>()), Times.Once);
        }

        [Fact]
        public async Task FinalizarCurso_DeveConcluirMatriculaEPublicarEvento()
        {
            // Arrange
            var aluno = new Aluno("Maria", "maria@email.com");
            var matricula = new Matricula(Guid.NewGuid());
            matricula.Historico.DefinirTotalAulas(3);
            matricula.Historico.RegistrarAulaConcluida();
            matricula.Historico.RegistrarAulaConcluida();
            matricula.Historico.RegistrarAulaConcluida();

            aluno.Matricular(matricula);

            _alunoRepositoryMock.Setup(r => r.ObterPorId(aluno.Id))
                .ReturnsAsync(aluno);
            _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(true);

            var command = new FinalizarCursoCommand(aluno.Id, matricula.Id);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(resultado);
            Assert.Equal(StatusMatricula.Concluida, matricula.Status);
            Assert.NotNull(matricula.Certificado);
            _mediatorHandlerMock.Verify(m => m.PublicarEvento(It.IsAny<CursoFinalizadoEvent>()), Times.Once);
        }

        [Fact]
        public async Task FinalizarCurso_DeveFalhar_SeAulasNaoConcluidas()
        {
            // Arrange
            var aluno = new Aluno("Lucas", "lucas@email.com");
            var matricula = new Matricula(Guid.NewGuid());
            matricula.Historico.DefinirTotalAulas(3);
            matricula.Historico.RegistrarAulaConcluida(); // incompleto

            aluno.Matricular(matricula);

            _alunoRepositoryMock.Setup(r => r.ObterPorId(aluno.Id))
                .ReturnsAsync(aluno);

            var command = new FinalizarCursoCommand(aluno.Id, matricula.Id);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(resultado);
            _mediatorHandlerMock.Verify(m => m.PublicarEvento(It.IsAny<CursoFinalizadoEvent>()), Times.Once);
            _mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
        }
    }
}

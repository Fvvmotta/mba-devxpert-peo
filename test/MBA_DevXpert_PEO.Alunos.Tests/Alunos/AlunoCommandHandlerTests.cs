using System;
using System.Threading;
using System.Threading.Tasks;
using Alunos.Commands;
using MBA_DevXpert_PEO.Alunos.Application.Commands;
using MBA_DevXpert_PEO.Alunos.Application.Events;
using MBA_DevXpert_PEO.Alunos.Application.Handlers;
using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using MBA_DevXpert_PEO.Alunos.Domain.Repositories;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace MBA_DevXpert_PEO.Tests.Alunos
{
    public class AlunoCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly AlunoCommandHandler _handler;

        public AlunoCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<AlunoCommandHandler>();
        }
        [Fact(DisplayName = "Criar Aluno com Sucesso")]
        [Trait("Categoria", "Aluno - Handler")]
        public async Task CriarAluno_DeveExecutarComSucesso()
        {
            // Arrange
            var command = new CriarAlunoCommand(Guid.NewGuid(), "João da Silva", "joao@email.com");

            _mocker.GetMock<IAlunoRepository>()
                   .Setup(r => r.UnitOfWork.Commit())
                   .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.Adicionar(It.IsAny<Aluno>()), Times.Once);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Criar Aluno Inválido")]
        [Trait("Categoria", "Aluno - Handler")]
        public async Task CriarAluno_CommandoInvalido_DeveFalhar()
        {
            // Arrange
            var command = new CriarAlunoCommand(Guid.Empty, "", "invalido");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.Adicionar(It.IsAny<Aluno>()), Times.Never);
            _mocker.GetMock<IMediatorHandler>().Verify(m =>
                m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Criar Aluno - Falha no Commit")]
        [Trait("Categoria", "Aluno - Handler")]
        public async Task CriarAluno_FalhaCommit_DeveRetornarFalse()
        {
            // Arrange
            var command = new CriarAlunoCommand(Guid.NewGuid(), "Aluno X", "teste@email.com");

            _mocker.GetMock<IAlunoRepository>()
                   .Setup(r => r.UnitOfWork.Commit())
                   .ReturnsAsync(false); // Simula falha no commit

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.Adicionar(It.IsAny<Aluno>()), Times.Once);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }
    
        [Fact(DisplayName = "Atualizar Aluno com Sucesso")]
        [Trait("Categoria", "Aluno - Command Handler")]
        public async Task AtualizarAluno_DeveAtualizarComSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var command = new AtualizarAlunoCommand(alunoId, "Aluno Atualizado", "teste@email.com");

            var alunoFake = Aluno.CriarComId(alunoId, "Aluno Teste", "aluno@teste.com");

            _mocker.GetMock<IAlunoRepository>().Setup(r => r.ObterPorId(alunoId)).ReturnsAsync(alunoFake);
            _mocker.GetMock<IAlunoRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.Atualizar(It.IsAny<Aluno>()), Times.Once);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Atualizar Aluno com Dados Inválidos")]
        [Trait("Categoria", "Aluno - Command Handler")]
        public async Task AtualizarAluno_ComandoInvalido_DeveRetornarFalseENotificar()
        {
            // Arrange
            var command = new AtualizarAlunoCommand(Guid.Empty, "", "email-invalido");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m =>
                m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Atualizar Aluno Não Encontrado")]
        [Trait("Categoria", "Aluno - Command Handler")]
        public async Task AtualizarAluno_AlunoNaoEncontrado_DeveRetornarFalse()
        {
            // Arrange
            var command = new AtualizarAlunoCommand(Guid.NewGuid(), "Aluno X", "teste@email.com");

            _mocker.GetMock<IAlunoRepository>().Setup(r => r.ObterPorId(command.AlunoId)).ReturnsAsync((Aluno)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m =>
                m.PublicarNotificacao(It.Is<DomainNotification>(n => n.Value == "Aluno não encontrado.")), Times.Once);
        }

        [Fact(DisplayName = "Criar Matricula com sucesso")]
        [Trait("Categoria", "Alunos - Matricula Handler")]
        public async Task Handle_CriarMatricula_DeveExecutarComSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var command = new CriarMatriculaCommand(alunoId, cursoId, 1000m, 20);

            var aluno = Aluno.CriarComId(alunoId, "Aluno Teste", "aluno@teste.com");

            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            alunoRepositoryMock.Setup(r => r.ObterPorId(alunoId)).ReturnsAsync(aluno);
            alunoRepositoryMock.Setup(r => r.AdicionarMatricula(It.IsAny<Matricula>()));
            alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            var mediatorMock = new Mock<IMediatorHandler>();

            var handler = new AlunoCommandHandler(alunoRepositoryMock.Object, mediatorMock.Object);

            // Act
            var resultado = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(resultado);
            alunoRepositoryMock.Verify(r => r.ObterPorId(alunoId), Times.Once);
            alunoRepositoryMock.Verify(r => r.AdicionarMatricula(It.IsAny<Matricula>()), Times.Once);
            alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
            mediatorMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Never);
        }

        [Fact(DisplayName = "Criar Matricula - Aluno inexistente")]
        [Trait("Categoria", "Alunos - Matricula Handler")]
        public async Task Handle_CriarMatricula_AlunoNaoEncontrado_DevePublicarNotificacao()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var command = new CriarMatriculaCommand(alunoId, cursoId, 1000m, 20);

            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            alunoRepositoryMock.Setup(r => r.ObterPorId(alunoId)).ReturnsAsync((Aluno)null);

            var mediatorMock = new Mock<IMediatorHandler>();

            var handler = new AlunoCommandHandler(alunoRepositoryMock.Object, mediatorMock.Object);

            // Act
            var resultado = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(resultado);
            mediatorMock.Verify(m => m.PublicarNotificacao(
                It.Is<DomainNotification>(n => n.Value == "Aluno não encontrado.")), Times.Once);
        }

        [Fact(DisplayName = "Criar Matricula - Comando Inválido")]
        [Trait("Categoria", "Alunos - Matricula Handler")]
        public async Task Handle_CriarMatricula_ComandoInvalido_DeveFalharValidacao()
        {
            // Arrange
            var command = new CriarMatriculaCommand(Guid.Empty, Guid.Empty, 0, 0);

            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            var mediatorMock = new Mock<IMediatorHandler>();

            var handler = new AlunoCommandHandler(alunoRepositoryMock.Object, mediatorMock.Object);

            // Act
            var resultado = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(resultado);
            mediatorMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
            alunoRepositoryMock.Verify(r => r.ObterPorId(It.IsAny<Guid>()), Times.Never);
        }
        [Fact(DisplayName = "Finalizar aula com sucesso")]
        [Trait("Categoria", "Alunos - Aula Handler")]
        public async Task Handle_FinalizarAula_DeveExecutarComSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var command = new FinalizarAulaCommand(alunoId, matriculaId, 10);

            var matricula = new Matricula(alunoId, Guid.NewGuid(), 1000);
            var aluno = Aluno.CriarComId(alunoId, "Aluno Teste", "aluno@teste.com");

            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            aluno.Matricular(matricula);

            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            alunoRepositoryMock.Setup(r => r.ObterPorId(alunoId)).ReturnsAsync(aluno);
            alunoRepositoryMock.Setup(r => r.Atualizar(It.IsAny<Aluno>()));
            alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            var mediatorMock = new Mock<IMediatorHandler>();

            var handler = new AlunoCommandHandler(alunoRepositoryMock.Object, mediatorMock.Object);

            // Act
            var resultado = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(resultado);
            alunoRepositoryMock.Verify(r => r.ObterPorId(alunoId), Times.Once);
            alunoRepositoryMock.Verify(r => r.Atualizar(It.IsAny<Aluno>()), Times.Once);
            alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
            mediatorMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Never);
        }
        [Fact(DisplayName = "Finalizar Curso com sucesso")]
        [Trait("Categoria", "Alunos - Curso Handler")]
        public async Task Handle_FinalizarCurso_DeveExecutarComSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var command = new FinalizarCursoCommand(
                alunoId,
                matriculaId,
                "Aluno Teste",
                "Curso Teste",
                60
            );

            var aluno = Aluno.CriarComId(alunoId, "Aluno Teste", "teste@aluno.com");
            var matricula = new Matricula(alunoId, Guid.NewGuid(), 1200);
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            aluno.Matricular(matricula);

            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            alunoRepositoryMock.Setup(r => r.ObterPorId(alunoId)).ReturnsAsync(aluno);
            alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            var mediatorMock = new Mock<IMediatorHandler>();

            var handler = new AlunoCommandHandler(alunoRepositoryMock.Object, mediatorMock.Object);
            matricula.DefinirTotalAulas(10);

            for (int i = 0; i < 10; i++)
                matricula.RegistrarAulaConcluida();
            // Act
            var resultado = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(resultado);
            alunoRepositoryMock.Verify(r => r.ObterPorId(alunoId), Times.Once);
            alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
            mediatorMock.Verify(m => m.PublicarEvento(It.IsAny<CursoFinalizadoEvent>()), Times.Once);
            mediatorMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Never);
        }

        [Fact(DisplayName = "Finalizar Curso com erro - Aluno não encontrado")]
        [Trait("Categoria", "Alunos - Curso Handler")]
        public async Task Handle_FinalizarCurso_AlunoNaoEncontrado_DevePublicarNotificacao()
        {
            // Arrange
            var command = new FinalizarCursoCommand(Guid.NewGuid(), Guid.NewGuid(), "Aluno", "Curso", 60);

            var alunoRepositoryMock = new Mock<IAlunoRepository>();
            alunoRepositoryMock.Setup(r => r.ObterPorId(It.IsAny<Guid>())).ReturnsAsync((Aluno)null);

            var mediatorMock = new Mock<IMediatorHandler>();

            var handler = new AlunoCommandHandler(alunoRepositoryMock.Object, mediatorMock.Object);

            // Act
            var resultado = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(resultado);
            mediatorMock.Verify(m => m.PublicarNotificacao(It.Is<DomainNotification>(n => n.Value == "Aluno não encontrado.")), Times.Once);
        }
    }
}

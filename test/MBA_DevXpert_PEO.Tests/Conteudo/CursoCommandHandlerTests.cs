using MBA_DevXpert_PEO.Conteudos.Application.Commands;
using MBA_DevXpert_PEO.Conteudos.Application.Handlers;
using MBA_DevXpert_PEO.Conteudos.Domain.Entities;
using MBA_DevXpert_PEO.Conteudos.Domain.Repositories;
using MBA_DevXpert_PEO.Conteudos.Domain.ValueObjects;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using Moq;
using Moq.AutoMock;


namespace MBA_DevXpert_PEO.Tests.Conteudo
{
    public class CursoCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly CursoCommandHandler _handler;

        public CursoCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<CursoCommandHandler>();
        }

        [Fact(DisplayName = "Criar Curso com Sucesso")]
        [Trait("Categoria", "Conteudo - Curso Command Handler")]
        public async Task CriarCurso_DeveExecutarComSucesso()
        {
            // Arrange
            var command = new CriarCursoCommand("Curso Teste", "Autor", 20, "Descrição");
            _mocker.GetMock<ICursoRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _mocker.GetMock<ICursoRepository>().Verify(r => r.Adicionar(It.IsAny<Curso>()), Times.Once);
            _mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Criar Curso Inválido")]
        [Trait("Categoria", "Conteudo - Curso Command Handler")]
        public async Task CriarCurso_CommandInvalido_DeveRetornarNull()
        {
            // Arrange
            var command = new CriarCursoCommand("", "", 0, "");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Adicionar Aula com Sucesso")]
        [Trait("Categoria", "Conteudo - Curso Command Handler")]
        public async Task AdicionarAula_DeveExecutarComSucesso()
        {
            // Arrange
            var curso = new Curso("Curso Teste", "Autor", 10, ConteudoProgramatico.Criar("Descrição"));
            var command = new AdicionarAulaCommand(curso.Id, "Título Aula", "Descrição Aula", "http://material.com");

            _mocker.GetMock<ICursoRepository>()
                   .Setup(r => r.ObterPorId(curso.Id))
                   .ReturnsAsync(curso);
            _mocker.GetMock<ICursoRepository>()
                   .Setup(r => r.UnitOfWork.Commit())
                   .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<ICursoRepository>().Verify(r => r.AdicionarAula(It.IsAny<Aula>()), Times.Once);
            _mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Aula em Curso Inexistente")]
        [Trait("Categoria", "Conteudo - Curso Command Handler")]
        public async Task AdicionarAula_CursoInexistente_DeveRetornarFalse()
        {
            // Arrange
            var command = new AdicionarAulaCommand(Guid.NewGuid(), "Título", "Descrição", "url");
            _mocker.GetMock<ICursoRepository>()
                   .Setup(r => r.ObterPorId(command.CursoId))
                   .ReturnsAsync((Curso)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m =>
                m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Aula com Command Inválido")]
        [Trait("Categoria", "Conteudo - Curso Command Handler")]
        public async Task AdicionarAula_CommandInvalido_DeveRetornarFalse()
        {
            // Arrange
            var command = new AdicionarAulaCommand(Guid.Empty, "", "", "");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m =>
                m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Atualizar Curso com Sucesso")]
        [Trait("Categoria", "Conteudo - Curso Command Handler")]
        public async Task AtualizarCurso_DeveExecutarComSucesso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var command = new AtualizarCursoCommand(
                cursoId,
                "Curso Atualizado",
                "Autor Atualizado",
                40,
                "Descrição atualizada do conteúdo"
            );

            var cursoMock = new Curso("Nome Original", "Autor Original", 20, ConteudoProgramatico.Criar("Conteúdo Original"));

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.ObterPorId(cursoId))
                .ReturnsAsync(cursoMock);

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<ICursoRepository>().Verify(r => r.Atualizar(It.IsAny<Curso>()), Times.Once);
            _mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Atualizar Curso Inválido")]
        [Trait("Categoria", "Conteudo - Curso Command Handler")]
        public async Task AtualizarCurso_CommandInvalido_DeveFalharENotificarErro()
        {
            // Arrange
            var command = new AtualizarCursoCommand(Guid.Empty, "", "", 0, "");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>()
                .Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Atualizar Curso Não Encontrado")]
        [Trait("Categoria", "Conteudo - Curso Command Handler")]
        public async Task AtualizarCurso_CursoNaoEncontrado_DeveFalharENotificar()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var command = new AtualizarCursoCommand(
                cursoId,
                "Curso Teste",
                "Autor",
                10,
                "Conteúdo"
            );

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.ObterPorId(cursoId))
                .ReturnsAsync((Curso)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>()
                .Verify(m => m.PublicarNotificacao(It.Is<DomainNotification>(n => n.Value == "Curso não encontrado.")), Times.Once);
        }

        [Fact(DisplayName = "Excluir Aula com Sucesso")]
        [Trait("Categoria", "Conteudo - Curso Command Handler")]
        public async Task ExcluirAula_DeveExecutarComSucesso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();

            var cursoMock = new Curso("Curso Teste", "Autor Teste", 10, ConteudoProgramatico.Criar("Descrição"));
            var aula = cursoMock.AdicionarAula("Aula Teste", "Descrição", "http://material.com");

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.ObterPorId(cursoId))
                .ReturnsAsync(cursoMock);

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(true);

            var command = new ExcluirAulaCursoCommand(cursoId, aula.Id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<ICursoRepository>().Verify(r => r.Atualizar(It.IsAny<Curso>()), Times.Once);
            _mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Excluir Aula com Curso Não Encontrado")]
        [Trait("Categoria", "Conteudo - Curso Command Handler")]
        public async Task ExcluirAula_CursoNaoEncontrado_DeveRetornarFalso()
        {
            // Arrange
            var command = new ExcluirAulaCursoCommand(Guid.NewGuid(), Guid.NewGuid());

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.ObterPorId(It.IsAny<Guid>()))
                .ReturnsAsync((Curso)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m =>
                m.PublicarNotificacao(It.Is<DomainNotification>(n => n.Value == "Curso não encontrado.")),
                Times.Once);
        }

        [Fact(DisplayName = "Excluir Aula Command Inválido")]
        [Trait("Categoria", "Conteudo - Curso Command Handler")]
        public async Task ExcluirAula_CommandInvalido_DeveRetornarFalso()
        {
            // Arrange
            var command = new ExcluirAulaCursoCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m =>
                m.PublicarNotificacao(It.Is<DomainNotification>(n => n.Value.Contains("Dados inválidos para exclusão de aula."))),
                Times.Once);
        }

        [Fact(DisplayName = "Excluir Curso com Sucesso")]
        [Trait("Categoria", "Conteudo - Curso Command Handler")]
        public async Task ExcluirCurso_DeveExecutarComSucesso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var command = new ExcluirCursoCommand(cursoId);
            var curso = new Curso("Curso Teste", "Autor", 10, ConteudoProgramatico.Criar("Descrição"));

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.ObterPorId(cursoId))
                .ReturnsAsync(curso);

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<ICursoRepository>().Verify(r => r.Remover(curso), Times.Once);
            _mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Excluir Curso Inexistente")]
        [Trait("Categoria", "Conteudo - Curso Command Handler")]
        public async Task ExcluirCurso_CursoNaoEncontrado_DeveRetornarFalseENotificar()
        {
            // Arrange
            var command = new ExcluirCursoCommand(Guid.NewGuid());

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.ObterPorId(command.Id))
                .ReturnsAsync((Curso)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m =>
                m.PublicarNotificacao(It.Is<DomainNotification>(n => n.Value == "Curso não encontrado.")),
                Times.Once);
        }

    }

}

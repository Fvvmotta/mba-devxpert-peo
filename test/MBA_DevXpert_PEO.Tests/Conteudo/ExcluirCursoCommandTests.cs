using System;
using System.Linq;
using MBA_DevXpert_PEO.Conteudos.Application.Commands;
using Xunit;

namespace MBA_DevXpert_PEO.Tests.Conteudo
{
    public class ExcluirCursoCommandTests
    {
        [Fact(DisplayName = "Excluir Curso Command Válido")]
        [Trait("Categoria", "Conteudo - Curso Commands")]
        public void ExcluirCursoCommand_CommandoValido_DevePassarNaValidacao()
        {
            // Arrange
            var command = new ExcluirCursoCommand(Guid.NewGuid());

            // Act
            var result = command.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Excluir Curso Command Inválido")]
        [Trait("Categoria", "Conteudo - Curso Commands")]
        public void ExcluirCursoCommand_CommandoInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var command = new ExcluirCursoCommand(Guid.Empty);

            // Act
            var result = command.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(DeleteAulaCursoValidation.CursoIdObrigatorioMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }
    }
}

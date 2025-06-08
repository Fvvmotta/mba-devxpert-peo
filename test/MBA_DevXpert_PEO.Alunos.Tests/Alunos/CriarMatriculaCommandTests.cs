using System;
using System.Linq;
using MBA_DevXpert_PEO.Alunos.Application.Commands;
using Xunit;

namespace MBA_DevXpert_PEO.Tests.Alunos
{
    public class CriarMatriculaCommandTests
    {
        [Fact(DisplayName = "Criar Matricula Command Válido")]
        [Trait("Categoria", "Alunos - Matricula Commands")]
        public void CriarMatriculaCommand_CommandValido_DevePassarNaValidacao()
        {
            // Arrange
            var command = new CriarMatriculaCommand(Guid.NewGuid(), Guid.NewGuid(), 1200.00m, 10);

            // Act
            var result = command.EhValido();

            // Assert
            Assert.True(result);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Criar Matricula Command Inválido")]
        [Trait("Categoria", "Alunos - Matricula Commands")]
        public void CriarMatriculaCommand_CommandInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var command = new CriarMatriculaCommand(Guid.Empty, Guid.Empty, 0, 0);

            // Act
            var result = command.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(CriarMatriculaValidation.AlunoIdErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarMatriculaValidation.CursoIdErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarMatriculaValidation.ValorErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarMatriculaValidation.TotalAulasErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }
    }
}

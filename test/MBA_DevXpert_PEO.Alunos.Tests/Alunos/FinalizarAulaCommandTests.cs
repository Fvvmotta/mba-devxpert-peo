using System;
using System.Linq;
using Alunos.Commands;
using Xunit;

namespace MBA_DevXpert_PEO.Tests.Alunos
{
    public class FinalizarAulaCommandTests
    {
        [Fact(DisplayName = "Finalizar Aula Command Válido")]
        [Trait("Categoria", "Alunos - Aula Commands")]
        public void FinalizarAulaCommand_ComandoValido_DevePassarNaValidacao()
        {
            // Arrange
            var command = new FinalizarAulaCommand(Guid.NewGuid(), Guid.NewGuid(), 10);

            // Act
            var result = command.EhValido();

            // Assert
            Assert.True(result);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Finalizar Aula Command Inválido")]
        [Trait("Categoria", "Alunos - Aula Commands")]
        public void FinalizarAulaCommand_ComandoInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var command = new FinalizarAulaCommand(Guid.Empty, Guid.Empty, 0);

            // Act
            var result = command.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(FinalizarAulaValidation.AlunoIdErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(FinalizarAulaValidation.MatriculaIdErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(FinalizarAulaValidation.TotalAulasErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }
    }
}

using System;
using System.Linq;
using MBA_DevXpert_PEO.Alunos.Application.Commands;
using Xunit;

namespace MBA_DevXpert_PEO.Tests.Alunos
{
    public class FinalizarCursoCommandTests
    {
        [Fact(DisplayName = "Finalizar Curso Command Válido")]
        [Trait("Categoria", "Alunos - Curso Commands")]
        public void FinalizarCursoCommand_CommandValido_DevePassarNaValidacao()
        {
            // Arrange
            var command = new FinalizarCursoCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Aluno Exemplo",
                "Curso Exemplo",
                40
            );

            // Act
            var result = command.EhValido();

            // Assert
            Assert.True(result);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Finalizar Curso Command Inválido")]
        [Trait("Categoria", "Alunos - Curso Commands")]
        public void FinalizarCursoCommand_CommandInvalido_DeveFalharNaValidacao()
        {
            // Arrange
            var command = new FinalizarCursoCommand(
                Guid.Empty,
                Guid.Empty,
                "",
                "",
                0
            );

            // Act
            var result = command.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(FinalizarCursoValidation.AlunoIdErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(FinalizarCursoValidation.MatriculaIdErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(FinalizarCursoValidation.AlunoNomeErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(FinalizarCursoValidation.CursoNomeErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(FinalizarCursoValidation.CargaHorariaErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }
    }
}

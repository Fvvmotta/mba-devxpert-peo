using System;
using System.Linq;
using MBA_DevXpert_PEO.Conteudos.Application.Commands;
using Xunit;

namespace MBA_DevXpert_PEO.Tests.Conteudo
{
    public class AtualizarCursoCommandTests
    {
        [Fact(DisplayName = "Atualizar Curso Command Válido")]
        [Trait("Categoria", "Conteudo - Curso Commands")]
        public void AtualizarCursoCommand_CommandValido_DevePassarNaValidacao()
        {
            // Arrange
            var command = new AtualizarCursoCommand(
                Guid.NewGuid(),
                "Curso Atualizado",
                "Autor Teste",
                30,
                "Descrição atualizada do curso"
            );

            // Act
            var result = command.EhValido();

            // Assert
            Assert.True(result);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Atualizar Curso Command Inválido")]
        [Trait("Categoria", "Conteudo - Curso Commands")]
        public void AtualizarCursoCommand_CommandInvalido_DeveFalharNaValidacao()
        {
            // Arrange
            var command = new AtualizarCursoCommand(
                Guid.Empty,
                "",
                "",
                0,
                ""
            );

            // Act
            var result = command.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(UpdateCursoValidation.IdObrigatorioMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(UpdateCursoValidation.NomeObrigatorioMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(UpdateCursoValidation.AutorObrigatorioMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(UpdateCursoValidation.CargaHorariaInvalidaMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(UpdateCursoValidation.DescricaoObrigatoriaMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }
    }
}

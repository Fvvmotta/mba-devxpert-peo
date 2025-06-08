using System;
using System.Linq;
using MBA_DevXpert_PEO.Conteudos.Application.Commands;
using Xunit;

namespace MBA_DevXpert_PEO.Tests.Conteudo
{
    public class CriarCursoCommandTests
    {
        [Fact(DisplayName = "Criar Curso Command Válido")]
        [Trait("Categoria", "Conteudo - Curso Commands")]
        public void CriarCursoCommand_CommandoValido_DevePassarNaValidacao()
        {
            // Arrange
            var command = new CriarCursoCommand("Curso de Teste", "Professor X", 40, "Conteúdo básico");

            // Act
            var result = command.EhValido();

            // Assert
            Assert.True(result);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Criar Curso Command Inválido")]
        [Trait("Categoria", "Conteudo - Curso Commands")]
        public void CriarCursoCommand_CommandoInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var command = new CriarCursoCommand("", "", 0, "");

            // Act
            var result = command.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(CriarCursoValidation.NomeObrigatorioMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarCursoValidation.AutorObrigatorioMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarCursoValidation.CargaHorariaInvalidaMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarCursoValidation.DescricaoObrigatoriaMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }
    }
}

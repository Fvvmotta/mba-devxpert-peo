using System;
using System.Linq;
using Xunit;
using MBA_DevXpert_PEO.Conteudos.Application.Commands;

namespace MBA_DevXpert_PEO.Tests.Conteudo
{
    public class AdicionarAulaCommandTests
    {
        [Fact(DisplayName = "Adicionar Aula Command Válido")]
        [Trait("Categoria", "Conteudo - Aula Commands")]
        public void AdicionarAulaCommand_ComandoValido_DeveSerValido()
        {
            // Arrange
            var command = new AdicionarAulaCommand(
                Guid.NewGuid(),
                "Introdução ao C#",
                "Nesta aula vamos aprender os conceitos básicos de C#",
                "https://meumaterial.com/csharp-introducao"
            );

            // Act
            var result = command.EhValido();

            // Assert
            Assert.True(result);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Adicionar Aula Command Inválido")]
        [Trait("Categoria", "Conteudo - Aula Commands")]
        public void AdicionarAulaCommand_ComandoInvalido_DeveFalharNaValidacao()
        {
            // Arrange
            var command = new AdicionarAulaCommand(
                Guid.Empty,
                "",
                "",
                ""
            );

            // Act
            var result = command.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(AdicionarAulaValidation.CursoIdObrigatorioMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(AdicionarAulaValidation.TituloObrigatorioMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(AdicionarAulaValidation.DescricaoObrigatoriaMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(AdicionarAulaValidation.UrlObrigatoriaMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }

        [Fact(DisplayName = "Adicionar Aula Command - Título maior que 100 caracteres")]
        [Trait("Categoria", "Conteudo - Aula Commands")]
        public void AdicionarAulaCommand_TituloMuitoLongo_DeveFalharNaValidacao()
        {
            // Arrange
            var titulo = new string('A', 101);
            var command = new AdicionarAulaCommand(
                Guid.NewGuid(),
                titulo,
                "Descrição válida",
                "https://meumaterial.com/material"
            );

            // Act
            var result = command.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(AdicionarAulaValidation.TituloMaximoMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }

        [Fact(DisplayName = "Adicionar Aula Command - URL maior que 200 caracteres")]
        [Trait("Categoria", "Conteudo - Aula Commands")]
        public void AdicionarAulaCommand_UrlMuitoLonga_DeveFalharNaValidacao()
        {
            // Arrange
            var url = "https://site.com/" + new string('x', 185);
            var command = new AdicionarAulaCommand(
                Guid.NewGuid(),
                "Título válido",
                "Descrição válida",
                url
            );

            // Act
            var result = command.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(AdicionarAulaValidation.UrlMaximoMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }
    }
}

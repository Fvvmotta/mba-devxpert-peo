using System;
using System.Linq;
using MBA_DevXpert_PEO.Alunos.Application.Commands;
using Xunit;

namespace MBA_DevXpert_PEO.Tests.Alunos
{
    public class AtualizarAlunoCommandTests
    {
        [Fact(DisplayName = "Atualizar Aluno Command Válido")]
        [Trait("Categoria", "Aluno - Commands")]
        public void AtualizarAlunoCommand_DeveSerValido_QuandoComandoEstaCorreto()
        {
            // Arrange
            var command = new AtualizarAlunoCommand(Guid.NewGuid(), "Aluno Teste", "teste@email.com");

            // Act
            var result = command.EhValido();

            // Assert
            Assert.True(result);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Atualizar Aluno Command Inválido")]
        [Trait("Categoria", "Aluno - Commands")]
        public void AtualizarAlunoCommand_DeveSerInvalido_QuandoDadosForemInvalidos()
        {
            // Arrange
            var command = new AtualizarAlunoCommand(Guid.Empty, "", "email-invalido");

            // Act
            var result = command.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(AtualizarAlunoValidation.IdAlunoErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(AtualizarAlunoValidation.NomeErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(AtualizarAlunoValidation.EmailInvalidoErroMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }
    }
}

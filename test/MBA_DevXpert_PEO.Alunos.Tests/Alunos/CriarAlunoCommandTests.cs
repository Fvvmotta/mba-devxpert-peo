using System;
using System.Linq;
using Alunos.Commands;
using MBA_DevXpert_PEO.Alunos.Application.Commands;
using Xunit;

namespace MBA_DevXpert_PEO.Tests.Alunos
{
    public class CriarAlunoCommandTests
    {
        [Fact(DisplayName = "Criar Aluno Command Válido")]
        [Trait("Categoria", "Aluno - Commands")]
        public void CriarAlunoCommand_ComandoValido_DevePassarNaValidacao()
        {
            // Arrange
            var command = new CriarAlunoCommand(Guid.NewGuid(), "Aluno Teste", "aluno@teste.com");

            // Act
            var result = command.EhValido();

            // Assert
            Assert.True(result);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Criar Aluno Command Inválido")]
        [Trait("Categoria", "Aluno - Commands")]
        public void CriarAlunoCommand_ComandoInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var command = new CriarAlunoCommand(Guid.Empty, "", "email_invalido");

            // Act
            var result = command.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(CriarAlunoValidation.IdObrigatorioMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarAlunoValidation.NomeObrigatorioMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(CriarAlunoValidation.EmailInvalidoMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }
    }
}

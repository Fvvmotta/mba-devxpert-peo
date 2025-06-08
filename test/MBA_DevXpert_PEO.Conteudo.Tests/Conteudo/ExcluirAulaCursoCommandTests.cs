using MBA_DevXpert_PEO.Conteudos.Application.Commands;

namespace MBA_DevXpert_PEO.Tests.Conteudo
{
    public class ExcluirAulaCursoCommandTests
    {
        [Fact(DisplayName = "Excluir Aula Command Válido")]
        [Trait("Categoria", "Conteudo - Aula Commands")]
        public void ExcluirAulaCursoCommand_ComandoValido_DevePassarNaValidacao()
        {
            // Arrange
            var command = new ExcluirAulaCursoCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = command.EhValido();

            // Assert
            Assert.True(result);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Excluir Aula Command Inválido")]
        [Trait("Categoria", "Conteudo - Aula Commands")]
        public void ExcluirAulaCursoCommand_ComandoInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var command = new ExcluirAulaCursoCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = command.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(DeleteAulaCursoValidation.CursoIdObrigatorioMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(DeleteAulaCursoValidation.AulaIdObrigatorioMsg, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }
    }
}

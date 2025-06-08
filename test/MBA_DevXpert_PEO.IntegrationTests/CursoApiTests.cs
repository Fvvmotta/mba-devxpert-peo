using System.Net;
using System.Net.Http.Json;
using MBA_DevXpert_PEO.Api.Tests.Config;
using MBA_DevXpert_PEO.Conteudos.Application.DTOs;
using MBA_DevXpert_PEO.Core.DomainObjects.DTO;
using Xunit;

namespace MBA_DevXpert_PEO.Api.Tests.Cursos
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class CursoApiTests
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public CursoApiTests(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Obter todos os cursos com sucesso")]
        [Trait("Categoria", "Integração API - Curso")]
        public async Task Curso_ObterTodos_DeveRetornarSucesso()
        {
            // Arrange
            _testsFixture.Client.AtribuirJsonMediaType();

            // Act
            var response = await _testsFixture.Client.GetAsync("api/curso");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var cursos = await response.Content.ReadFromJsonAsync<IEnumerable<CursoDto>>();
            Assert.NotNull(cursos);
        }

        [Fact(DisplayName = "Obter curso por ID inválido")]
        [Trait("Categoria", "Integração API - Curso")]
        public async Task Curso_ObterPorId_Inexistente_DeveRetornarErro()
        {
            // Arrange
            var cursoIdInvalido = Guid.NewGuid();
            _testsFixture.Client.AtribuirJsonMediaType();

            // Act
            var response = await _testsFixture.Client.GetAsync($"api/curso/{cursoIdInvalido}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var erro = await response.Content.ReadAsStringAsync();
            Assert.Contains("Curso não encontrado", erro);
        }
    }
}

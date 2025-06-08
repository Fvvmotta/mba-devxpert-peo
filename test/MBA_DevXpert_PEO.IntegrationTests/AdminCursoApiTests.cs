using System.Net.Http.Json;
using MBA_DevXpert_PEO.Api.Tests.Config;
using MBA_DevXpert_PEO.Conteudos.Application.DTOs;
using Xunit;

namespace MBA_DevXpert_PEO.Api.Tests.CursoAdmin;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class AdminCursoApiTests
{
    private readonly IntegrationTestsFixture<Program> _fixture;
    private readonly HttpClient _client;

    public AdminCursoApiTests(IntegrationTestsFixture<Program> fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }

    [Fact(DisplayName = "Criar curso com sucesso")]
    [Trait("Categoria", "Integração API - AdminCurso")]
    public async Task CriarCurso_DeveRetornarSucesso()
    {
        // Arrange
        await _fixture.RealizarLoginApi();
        _client.AtribuirToken(_fixture.UsuarioToken);

        var dto = new AdicionarCursoDto
        {
            Nome = "Curso Integração",
            Autor = "Professor X",
            CargaHoraria = 20,
            DescricaoConteudoProgramatico = "Programação, Testes"
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/admincurso", dto);

        // Assert
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadFromJsonAsync<CursoCriadoResponse>();
        Assert.NotNull(responseBody);
        Assert.NotEqual(Guid.Empty, responseBody.Id);
    }
    [Fact(DisplayName = "Adicionar aula com sucesso")]
    [Trait("Categoria", "Integração API - AdminCurso")]
    public async Task AdicionarAula_DeveRetornarSucesso()
    {
        // Arrange
        await _fixture.RealizarLoginApi();
        _client.AtribuirToken(_fixture.UsuarioToken);

        // Cria um curso para vincular a aula
        var cursoId = await _fixture.CriarCursoDeTesteAsync(_client);

        var dto = new AdicionarAulaDto
        {
            CursoId = cursoId,
            Titulo = "Aula 01 - Introdução",
            Descricao = "Conteúdo introdutório",
            MaterialUrl = "https://exemplo.com/material/introducao.pdf"
        };

        // Act
        var response = await _client.PostAsJsonAsync($"api/admincurso/{cursoId}/aulas", dto);

        // Assert
        response.EnsureSuccessStatusCode();
        var resultado = await response.Content.ReadAsStringAsync();
        Assert.Contains("sucesso", resultado, StringComparison.OrdinalIgnoreCase);
    }

    [Fact(DisplayName = "Atualizar curso com sucesso")]
    [Trait("Categoria", "Integração API - AdminCurso")]
    public async Task AtualizarCurso_DeveRetornarSucesso()
    {
        // Arrange
        await _fixture.RealizarLoginApi();
        _client.AtribuirToken(_fixture.UsuarioToken);

        var cursoId = await _fixture.CriarCursoDeTesteAsync(_client);

        var dto = new AtualizarCursoDto
        {
            Id = cursoId,
            Nome = "Curso Atualizado",
            Autor = "Prof. Alterado",
            CargaHoraria = 25,
            DescricaoConteudoProgramatico = "Conteúdo atualizado"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"api/admincurso/{cursoId}", dto);

        // Assert
        response.EnsureSuccessStatusCode();
        var resultado = await response.Content.ReadAsStringAsync();
        Assert.Contains("sucesso", resultado, StringComparison.OrdinalIgnoreCase);
    }

    [Fact(DisplayName = "Deletar curso com sucesso")]
    [Trait("Categoria", "Integração API - AdminCurso")]
    public async Task DeletarCurso_DeveRetornarSucesso()
    {
        // Arrange
        await _fixture.RealizarLoginApi();
        _client.AtribuirToken(_fixture.UsuarioToken);

        var cursoId = await _fixture.CriarCursoDeTesteAsync(_client);

        // Act
        var response = await _client.DeleteAsync($"api/admincurso/{cursoId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var resultado = await response.Content.ReadAsStringAsync();
        Assert.Contains("sucesso", resultado, StringComparison.OrdinalIgnoreCase);
    }
}

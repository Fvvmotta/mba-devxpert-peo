using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using MBA_DevXpert_PEO.Api;
using System.Text.Json;
using MBA_DevXpert_PEO.Conteudos.Application.DTOs;

namespace MBA_DevXpert_PEO.Api.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>> { }

    public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
    {
        public readonly DevXpertPEOAppFactory<TProgram> Factory;
        public HttpClient Client;

        public string UsuarioToken;

        public IntegrationTestsFixture()
        {
            Factory = new DevXpertPEOAppFactory<TProgram>();
            Client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost"),
                AllowAutoRedirect = false
            });
        }

        public async Task RealizarLoginApi()
        {
            var login = new
            {
                Email = "aluno@teste.com",
                Password = "Aluno@123"
            };

            var response = await Client.PostAsJsonAsync("auth/login", login);

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Resposta da API: {responseString}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao fazer login: {response.StatusCode} - {responseString}");
            }

            var data = JsonSerializer.Deserialize<TokenResponse>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            UsuarioToken = data?.Token?.Result ?? throw new Exception("Token não retornado.");

        }

        public async Task<Guid> CriarCursoDeTesteAsync(HttpClient client)
        {
            var dto = new AdicionarCursoDto
            {
                Nome = "Curso Teste",
                Autor = "Teste",
                CargaHoraria = 10,
                DescricaoConteudoProgramatico = "Conteúdo teste"
            };

            var response = await client.PostAsJsonAsync("api/admincurso", dto);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadFromJsonAsync<CursoCriadoResponse>();
            return body.Id;
        }

        public async Task<Guid> AdicionarAulaAoCursoAsync(HttpClient client, Guid cursoId)
        {
            var dto = new AdicionarAulaDto
            {
                CursoId = cursoId,
                Titulo = "Aula Teste",
                Descricao = "Descrição teste",
                MaterialUrl = "https://exemplo.com/teste.pdf"
            };

            var response = await client.PostAsJsonAsync($"api/admincurso/{cursoId}/aulas", dto);
            response.EnsureSuccessStatusCode();

            return Guid.NewGuid(); 
        }
        public class TokenResponse
        {
            public TokenWrapper Token { get; set; }
        }

        public class TokenWrapper
        {
            public string Result { get; set; }
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}

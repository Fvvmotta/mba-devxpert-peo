using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MBA_DevXpert_PEO.Tests.Integration.Conteudos.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;


namespace MBA_DevXpert_PEO.Tests.Integration.Cursos
{
    public class CursosApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CursosApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Deve_criar_um_novo_curso_via_API()
        {
            var command = new
            {
                Nome = "Curso Teste",
                Autor = "Lumar",
                CargaHoraria = 30,
                DescricaoConteudoProgramatico = "Descrição"
            };

            var response = await _client.PostAsJsonAsync("/api/curso", command);

            response.EnsureSuccessStatusCode();
        }
    }
}

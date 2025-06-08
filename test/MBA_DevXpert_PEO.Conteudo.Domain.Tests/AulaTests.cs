using System;
using System.Linq;
using Xunit;
using MBA_DevXpert_PEO.Conteudos.Domain.Entities;
using MBA_DevXpert_PEO.Conteudos.Domain.ValueObjects;
using MBA_DevXpert_PEO.Core.DomainObjects;

namespace MBA_DevXpert_PEO.Tests.Conteudos
{
    public class AulaTests
    {
        [Fact(DisplayName = "Criar aula válida deve funcionar")]
        public void Aula_Valida_DeveCriarComSucesso()
        {
            // Act
            var aula = new Aula("Titulo Aula", "Descricao Aula", "http://material.com");

            // Assert
            Assert.Equal("Titulo Aula", aula.Titulo);
            Assert.Equal("Descricao Aula", aula.Descricao);
            Assert.Equal("http://material.com", aula.MaterialUrl);
        }

        [Theory(DisplayName = "Criar aula com dados inválidos deve lançar exceção")]
        [InlineData("", "Descricao")]
        [InlineData("Titulo", "")]
        [InlineData(null, "Descricao")]
        [InlineData("Titulo", null)]
        public void Aula_DadosInvalidos_DeveLancarExcecao(string titulo, string descricao)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Aula(titulo, descricao));
        }
    }
}

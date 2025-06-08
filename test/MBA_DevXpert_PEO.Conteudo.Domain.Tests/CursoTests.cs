using System;
using System.Linq;
using Xunit;
using MBA_DevXpert_PEO.Conteudos.Domain.Entities;
using MBA_DevXpert_PEO.Conteudos.Domain.ValueObjects;
using MBA_DevXpert_PEO.Core.DomainObjects;

namespace MBA_DevXpert_PEO.Tests.Conteudos
{
    public class CursoTests
    {
        [Fact(DisplayName = "Criar curso válido deve funcionar")]
        public void Curso_Valido_DeveCriarComSucesso()
        {
            // Arrange
            var conteudo = ConteudoProgramatico.Criar("Curso introdutório de arquitetura de software.");

            // Act
            var curso = new Curso("Curso Teste", "Autor Teste", 40, conteudo);

            // Assert
            Assert.Equal("Curso Teste", curso.Nome);
            Assert.Equal("Autor Teste", curso.Autor);
            Assert.Equal(40, curso.CargaHoraria);
            Assert.Equal(conteudo, curso.ConteudoProgramatico);
            Assert.Empty(curso.Aulas);
        }

        [Fact(DisplayName = "Adicionar aula ao curso")]
        public void Curso_AdicionarAula_DeveAdicionarComSucesso()
        {
            // Arrange
            var conteudo = ConteudoProgramatico.Criar("Curso introdutório de arquitetura de software.");
            var curso = new Curso("Curso Teste", "Autor Teste", 40, conteudo);

            // Act
            var aula = curso.AdicionarAula("Aula 1", "Descricao 1", "http://material.com");

            // Assert
            Assert.Single(curso.Aulas);
            Assert.Equal("Aula 1", aula.Titulo);
        }

        [Fact(DisplayName = "Remover aula do curso")]
        public void Curso_RemoverAula_DeveRemoverComSucesso()
        {
            // Arrange
            var conteudo = ConteudoProgramatico.Criar("Curso introdutório de arquitetura de software.");
            var curso = new Curso("Curso Teste", "Autor Teste", 40, conteudo);
            var aula = curso.AdicionarAula("Aula 1", "Descricao 1", "http://material.com");

            // Act
            curso.RemoverAula(aula.Id);

            // Assert
            Assert.Empty(curso.Aulas);
        }

        [Fact(DisplayName = "Remover aula inexistente deve lançar exceção")]
        public void Curso_RemoverAulaInexistente_DeveLancarExcecao()
        {
            // Arrange
            var conteudo = ConteudoProgramatico.Criar("Curso introdutório de arquitetura de software.");
            var curso = new Curso("Curso Teste", "Autor Teste", 40, conteudo);

            // Act & Assert
            Assert.Throws<DomainException>(() => curso.RemoverAula(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Atualizar dados do curso com sucesso")]
        public void Curso_AtualizarCurso_DeveAtualizarComSucesso()
        {
            // Arrange
            var conteudo = ConteudoProgramatico.Criar("Curso introdutório de arquitetura de software.");
            var curso = new Curso("Curso Teste", "Autor Teste", 40, conteudo);
            var novoConteudo = ConteudoProgramatico.Criar("Curso intermediário de arquitetura de software.");

            // Act
            curso.AtualizarCurso("Novo Curso", 60, "Novo Autor", novoConteudo);

            // Assert
            Assert.Equal("Novo Curso", curso.Nome);
            Assert.Equal("Novo Autor", curso.Autor);
            Assert.Equal(60, curso.CargaHoraria);
            Assert.Equal(novoConteudo, curso.ConteudoProgramatico);
        }
    }
}

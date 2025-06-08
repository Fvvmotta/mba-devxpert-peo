using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using MBA_DevXpert_PEO.Core.DomainObjects;

namespace MBA_DevXpert_PEO.Alunos.Domain.Tests
{
    public class AlunoTests
    {
        [Fact(DisplayName = "Criar aluno com dados válidos")]
        public void CriarAluno_Valido_DeveInstanciarCorretamente()
        {
            var aluno = new Aluno("João", "joao@email.com");

            Assert.Equal("João", aluno.Nome);
            Assert.Equal("joao@email.com", aluno.Email);
            Assert.NotEqual(Guid.Empty, aluno.Id);
        }

        [Fact(DisplayName = "Atualizar dados do aluno")]
        public void AtualizarDados_DeveAtualizarNomeEEmail()
        {
            var aluno = new Aluno("João", "joao@email.com");
            aluno.AtualizarDados("Carlos", "carlos@email.com");

            Assert.Equal("Carlos", aluno.Nome);
            Assert.Equal("carlos@email.com", aluno.Email);
        }

        [Fact(DisplayName = "Não deve permitir matrícula duplicada no mesmo curso")]
        public void Matricular_MesmoCurso_DeveLancarExcecao()
        {
            var aluno = new Aluno("João", "joao@email.com");
            var matricula = new Matricula(aluno.Id, Guid.NewGuid(), 500);

            aluno.Matricular(matricula);

            var duplicada = new Matricula(aluno.Id, matricula.CursoId, 500);

            Assert.Throws<DomainException>(() => aluno.Matricular(duplicada));
        }

        [Fact(DisplayName = "Concluir matrícula com sucesso")]
        public void ConcluirMatricula_DeveConcluirQuandoTodasAulasCompletas()
        {
            var aluno = new Aluno("João", "joao@email.com");
            var matricula = new Matricula(aluno.Id, Guid.NewGuid(), 600);
            matricula.DefinirTotalAulas(2);
            matricula.RegistrarAulaConcluida();
            matricula.RegistrarAulaConcluida();
            aluno.Matricular(matricula);

            var sucesso = aluno.ConcluirMatricula(matricula.Id, aluno.Nome, "Curso Teste", 40, DateTime.UtcNow, out var erro);

            Assert.True(sucesso);
            Assert.True(string.IsNullOrEmpty(erro));
            Assert.NotNull(matricula.Certificado);
        }
    }
}

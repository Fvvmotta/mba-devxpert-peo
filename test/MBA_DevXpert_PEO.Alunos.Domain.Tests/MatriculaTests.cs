using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Alunos.Domain.Entities.Enum;

namespace MBA_DevXpert_PEO.Alunos.Domain.Tests
{
    public class MatriculaTests
    {
        [Fact(DisplayName = "Confirmar pagamento com sucesso")]
        public void ConfirmarPagamento_DeveAtivarMatricula()
        {
            var matricula = new Matricula(Guid.NewGuid(), Guid.NewGuid(), 600);

            var resultado = matricula.ConfirmarPagamento(out var erro);

            Assert.True(resultado);
            Assert.Equal(StatusMatricula.Ativa, matricula.Status);
            Assert.Null(erro);
        }

        [Fact(DisplayName = "Recusar pagamento com sucesso")]
        public void RecusarPagamento_DeveMarcarComoRecusado()
        {
            var matricula = new Matricula(Guid.NewGuid(), Guid.NewGuid(), 600);

            var resultado = matricula.RecusarPagamento(out var erro);

            Assert.True(resultado);
            Assert.Equal(StatusMatricula.PagamentoRecusado, matricula.Status);
            Assert.Null(erro);
        }

        [Fact(DisplayName = "Não deve concluir matrícula com aulas pendentes")]
        public void ConcluirMatricula_ComAulasPendentes_DeveRetornarErro()
        {
            var matricula = new Matricula(Guid.NewGuid(), Guid.NewGuid(), 600);
            matricula.DefinirTotalAulas(3);
            matricula.RegistrarAulaConcluida();

            var sucesso = matricula.Concluir("Aluno", "Curso", 40, DateTime.UtcNow, out var erro);

            Assert.False(sucesso);
            Assert.Equal("Nem todas as aulas foram concluídas.", erro);
        }
    }
}

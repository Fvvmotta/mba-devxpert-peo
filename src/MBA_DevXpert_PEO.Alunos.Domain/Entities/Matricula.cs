using MBA_DevXpert_PEO.Core.DomainObjects;
using MBA_DevXpert_PEO.Alunos.Domain.Entities.Enum;
using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using MBA_DevXpert_PEO.Alunos.Domain.ValueObjects;

public class Matricula : Entity
{
    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public decimal ValorCurso { get; private set; }
    public DateTime DataMatricula { get; private set; }
    public StatusMatricula Status { get; private set; }

    public Aluno Aluno { get; private set; }

    public Certificado? Certificado { get; private set; }
    public HistoricoAprendizado Historico { get; private set; }

    protected Matricula() { }

    public Matricula(Guid alunoId, Guid cursoId, decimal valorCurso)
    {
        Id = Guid.NewGuid();
        AlunoId = alunoId;
        CursoId = cursoId;
        ValorCurso = valorCurso;
        DataMatricula = DateTime.UtcNow;
        Status = StatusMatricula.PendentePagamento;
        Historico = new HistoricoAprendizado(0, 0);
    }

    public void DefinirTotalAulas(int total)
    {
        Historico = Historico.DefinirTotalAulas(total);
    }

    public void RegistrarAulaConcluida()
    {
        Historico = Historico.RegistrarAulaConcluida();
    }

    public bool ConfirmarPagamento(out string? erro)
    {
        if (Status == StatusMatricula.Ativa || Status == StatusMatricula.Concluida)
        {
            erro = "A matrícula já está ativa ou concluída.";
            return false;
        }
        Status = StatusMatricula.Ativa;
        erro = null;
        return true;
    }

    public bool RecusarPagamento(out string? erro)
    {
        if (Status == StatusMatricula.Ativa || Status == StatusMatricula.Concluida)
        {
            erro = "A matrícula já está ativa ou concluída. Não é possível recusar o pagamento.";
            return false;
        }
        Status = StatusMatricula.PagamentoRecusado;
        erro = null;
        return true;
    }


    public bool Concluir(string nomeAluno, string nomeCurso, int cargaHorariaCurso, DateTime dataConclusao, out string erro)
    {
        erro = string.Empty;

        if (!Historico.TodasAulasConcluidas)
        {
            erro = "Nem todas as aulas foram concluídas.";
            return false;
        }

        Status = StatusMatricula.Concluida;
        Certificado = new Certificado(Id, nomeAluno, nomeCurso, cargaHorariaCurso, dataConclusao);
        return true;
    }


}

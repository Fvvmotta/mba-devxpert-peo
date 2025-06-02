using MBA_DevXpert_PEO.Core.DomainObjects;
using MBA_DevXpert_PEO.Alunos.Domain.Entities.Enum;
using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using MBA_DevXpert_PEO.Alunos.Domain.ValueObjects;

public class Matricula : Entity
{
    public Guid CursoId { get; private set; }
    public DateTime DataMatricula { get; private set; }
    public StatusMatricula Status { get; private set; }

    public Certificado? Certificado { get; private set; }
    public HistoricoAprendizado Historico { get; private set; }

    protected Matricula() { }

    public Matricula(Guid cursoId)
    {
        Id = Guid.NewGuid();
        CursoId = cursoId;
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

    public void ConfirmarPagamento()
    {
        if (Status != StatusMatricula.PendentePagamento)
            throw new InvalidOperationException("Pagamento já processado.");

        Status = StatusMatricula.Ativa;
    }

    public void Concluir()
    {
        if (!Historico.TodasAulasConcluidas)
            throw new InvalidOperationException("Nem todas as aulas foram concluídas.");

        Status = StatusMatricula.Concluida;
        Certificado = new Certificado(Id);
    }
}

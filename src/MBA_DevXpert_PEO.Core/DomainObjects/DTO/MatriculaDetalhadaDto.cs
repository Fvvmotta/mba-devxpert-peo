using MBA_DevXpert_PEO.Core.DomainObjects.DTO;

public class MatriculaDetalhadaDto
{
    public Guid MatriculaId { get; set; }
    public Guid AlunoId { get; set; }
    public string AlunoNome { get; set; }

    public Guid CursoId { get; set; }
    public string CursoNome { get; set; }

    public DateTime DataMatricula { get; set; }
    public string Status { get; set; }

    public CertificadoDto? Certificado { get; set; }
    public PagamentoDto? Pagamento { get; set; }
}

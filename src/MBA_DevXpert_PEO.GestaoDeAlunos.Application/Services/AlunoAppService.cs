using MBA_DevXpert_PEO.Core.DomainObjects.DTO;
using MBA_DevXpert_PEO.Core.DomainObjects.Services;
using MBA_DevXpert_PEO.GestaoDeAlunos.Domain.Repositories;

public class AlunoAppService : IAlunoAppService
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly IPagamentoConsultaService _pagamentoConsulta;
    private readonly ICursoConsultaService _cursoConsulta;

    public AlunoAppService(
        IAlunoRepository alunoRepository,
        IPagamentoConsultaService pagamentoConsulta,
        ICursoConsultaService cursoConsulta)
    {
        _alunoRepository = alunoRepository;
        _pagamentoConsulta = pagamentoConsulta;
        _cursoConsulta = cursoConsulta;
    }

    public async Task<IEnumerable<MatriculaDetalhadaDto>> ObterMatriculasDetalhadas()
    {
        var alunos = await _alunoRepository.ObterTodosComMatriculas();

        var matriculaIds = alunos
            .SelectMany(a => a.Matriculas)
            .Select(m => m.Id)
            .ToList();

        var pagamentos = await _pagamentoConsulta.ObterPagamentosPorMatriculas(matriculaIds);
        var cursos = await _cursoConsulta.ObterTodos();

        var resultado = new List<MatriculaDetalhadaDto>();

        foreach (var aluno in alunos)
        {
            foreach (var matricula in aluno.Matriculas)
            {
                var pagamento = pagamentos.FirstOrDefault(p => p.MatriculaId == matricula.Id);
                var curso = cursos.FirstOrDefault(c => c.Id == matricula.CursoId);

                var dto = new MatriculaDetalhadaDto
                {
                    MatriculaId = matricula.Id,
                    AlunoId = aluno.Id,
                    AlunoNome = aluno.Nome,
                    CursoId = matricula.CursoId,
                    CursoNome = curso?.Nome ?? "",
                    DataMatricula = matricula.DataMatricula,
                    Status = matricula.Status.ToString(),

                    Certificado = matricula.Certificado is not null
                        ? new CertificadoDto
                        {
                            Id = matricula.Certificado.Id,
                            MatriculaId = matricula.Id,
                            DataEmissao = matricula.Certificado.DataEmissao
                        }
                        : null,

                    Pagamento = pagamento is not null
                        ? new PagamentoDto
                        {
                            Id = pagamento.Id,
                            MatriculaId = pagamento.MatriculaId,
                            Valor = pagamento.Valor,
                            DataPagamento = pagamento.DataPagamento,
                            Status = pagamento.Status
                        }
                        : null
                };

                resultado.Add(dto);
            }
        }

        return resultado;
    }
}

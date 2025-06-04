using MBA_DevXpert_PEO.Core.DomainObjects.DTO;
using MBA_DevXpert_PEO.Alunos.Domain.Repositories;
using MBA_DevXpert_PEO.Alunos.Application.DTOs;

public class AlunoAppService : IAlunoAppService
{
    private readonly IAlunoRepository _alunoRepository;
    public AlunoAppService(
        IAlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    public async Task<IEnumerable<AlunoComMatriculasDto>> ObterAlunosComMatriculas()
    {
        var alunos = await _alunoRepository.ObterTodosComMatriculas();

        return alunos.Select(aluno => new AlunoComMatriculasDto
        {
            AlunoId = aluno.Id,
            Nome = aluno.Nome,
            Matriculas = aluno.Matriculas.Select(m => new MatriculaDto
            {
                Id = m.Id,
                CursoId = m.CursoId,
                DataMatricula = m.DataMatricula,
                Status = m.Status.ToString(),
                Certificado = m.Certificado is not null
                    ? new CertificadoDto
                    {
                        NomeAluno = m.Certificado.NomeAluno,
                        NomeCurso = m.Certificado.NomeCurso,
                        CargaHorariaCurso =m.Certificado.CargaHorariaCurso,
                        DataConclusao = m.Certificado.DataConclusao,
                        DataEmissao = m.Certificado.DataEmissao

                    }
                    : null
            }).ToList()
        });
    }
}

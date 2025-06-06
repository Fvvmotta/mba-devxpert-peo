using MBA_DevXpert_PEO.Alunos.Application.DTOs;

namespace Alunos.Queries
{
    public interface IAlunoQueries
    {
        Task<AlunoDto> ObterPorId(Guid alunoId);
        Task<MatriculaDto> ObterMatriculaPorId(Guid matriculaId);
        Task<IEnumerable<AlunoComMatriculasDto>> ObterAlunosComMatriculas();
        Task<MatriculaComCursoDto?> ObterMatriculaComCurso(Guid alunoId, Guid matriculaId);
        Task<CertificadoDto?> ObterCertificado(Guid alunoId, Guid matriculaId);

    }
}


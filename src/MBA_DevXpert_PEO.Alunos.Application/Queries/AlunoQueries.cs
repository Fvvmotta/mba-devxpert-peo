using Alunos.Queries;
using MBA_DevXpert_PEO.Alunos.Application.DTOs;
using MBA_DevXpert_PEO.Alunos.Infra.Context;
using Microsoft.EntityFrameworkCore;

public class AlunoQueries : IAlunoQueries
{
    private readonly AlunosContext _alunoContext;

    public AlunoQueries(AlunosContext alunoContext)
    {
        _alunoContext = alunoContext;
    }

    public async Task<AlunoDto> ObterPorId(Guid alunoId)
    {
        return await _alunoContext.Alunos
            .Where(a => a.Id == alunoId)
            .Select(a => new AlunoDto
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Email = a.Email
                })
                .FirstOrDefaultAsync();
    }
    public async Task<MatriculaComCursoDto?> ObterMatriculaComCurso(Guid alunoId, Guid matriculaId)
    {
        return await _alunoContext.Matriculas
            .Where(m => m.Id == matriculaId && m.AlunoId == alunoId)
            .Select(m => new MatriculaComCursoDto
            {
                MatriculaId = m.Id,
                CursoId = m.CursoId,
                DataMatricula = m.DataMatricula
            })
            .FirstOrDefaultAsync();
    }
    public async Task<CertificadoDto?> ObterCertificado(Guid alunoId, Guid matriculaId)
    {
        return await _alunoContext.Alunos
            .Include(a => a.Matriculas)
            .ThenInclude(m => m.Certificado)
            .Where(a => a.Id == alunoId)
            .SelectMany(a => a.Matriculas
                .Where(m => m.Id == matriculaId && m.Certificado != null)
                .Select(m => new CertificadoDto
                {
                    NomeAluno = m.Certificado.NomeAluno,
                    NomeCurso = m.Certificado.NomeCurso,
                    CargaHorariaCurso = m.Certificado.CargaHorariaCurso,
                    DataConclusao = m.Certificado.DataConclusao,
                    DataEmissao = m.Certificado.DataEmissao
                }))
            .FirstOrDefaultAsync();
    }

}

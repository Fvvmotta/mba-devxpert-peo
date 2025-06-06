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
            .AsNoTracking()
            .Include(a => a.Matriculas)
            .Where(a => a.Id == alunoId)
            .Select(a => new AlunoDto
            {
                Id = a.Id,
                Nome = a.Nome,
                Email = a.Email,
                Matriculas = a.Matriculas.Select(m => new MatriculaCompletaDto
                {
                    Id = m.Id,
                    CursoId = m.CursoId,
                    Valor = m.ValorCurso,
                    DataMatricula = m.DataMatricula,
                    Status = m.Status.ToString(),
                    Historico = new HistoricoAprendizadoDto
                    {
                        TotalAulas = m.Historico.TotalAulas,
                        AulasConcluidas = m.Historico.AulasConcluidas,
                        TodasAulasConcluidas = m.Historico.TodasAulasConcluidas
                    }
                }).ToList()
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
    public async Task<MatriculaDto?> ObterMatriculaPorId(Guid matriculaId)
    {
        return await _alunoContext.Matriculas
            .Where(m => m.Id == matriculaId)
            .Select(m => new MatriculaDto
            {
                Id = m.Id,
                CursoId = m.CursoId,
                AlunoId = m.AlunoId,
                Valor = m.ValorCurso,
                Status = m.Status.ToString()
            })
            .FirstOrDefaultAsync();
    }


}

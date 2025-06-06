using Alunos.Queries;
using MBA_DevXpert_PEO.Alunos.Application.DTOs;
using MBA_DevXpert_PEO.Alunos.Domain.Repositories;
using MBA_DevXpert_PEO.Alunos.Infra.Context;
using Microsoft.EntityFrameworkCore;

public class AlunoQueries : IAlunoQueries
{
    private readonly IAlunoRepository _alunoRepository;

    public AlunoQueries(IAlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    public async Task<AlunoDto?> ObterPorId(Guid alunoId)
    {
        var aluno = await _alunoRepository.ObterPorId(alunoId);
        if (aluno == null) return null;

        return new AlunoDto
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            Email = aluno.Email,
            Matriculas = aluno.Matriculas.Select(m => new MatriculaCompletaDto
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
        };
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
                        CargaHorariaCurso = m.Certificado.CargaHorariaCurso,
                        DataConclusao = m.Certificado.DataConclusao,
                        DataEmissao = m.Certificado.DataEmissao
                    }
                    : null
            }).ToList()
        });
    }

    public async Task<MatriculaDto?> ObterMatriculaPorId(Guid matriculaId)
    {
        var alunos = await _alunoRepository.ObterTodosComMatriculas();

        var matricula = alunos
            .SelectMany(a => a.Matriculas)
            .FirstOrDefault(m => m.Id == matriculaId);

        if (matricula == null) return null;

        return new MatriculaDto
        {
            Id = matricula.Id,
            AlunoId = matricula.AlunoId,
            CursoId = matricula.CursoId,
            Valor = matricula.ValorCurso,
            Status = matricula.Status.ToString()
        };
    }

    public async Task<MatriculaComCursoDto?> ObterMatriculaComCurso(Guid alunoId, Guid matriculaId)
    {
        var aluno = await _alunoRepository.ObterPorId(alunoId);
        if (aluno == null) return null;

        var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == matriculaId);
        if (matricula == null) return null;

        return new MatriculaComCursoDto
        {
            MatriculaId = matricula.Id,
            CursoId = matricula.CursoId,
            DataMatricula = matricula.DataMatricula
        };
    }

    public async Task<CertificadoDto?> ObterCertificado(Guid alunoId, Guid matriculaId)
    {
        var aluno = await _alunoRepository.ObterPorId(alunoId);
        if (aluno == null) return null;

        var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == matriculaId && m.Certificado != null);
        if (matricula?.Certificado == null) return null;

        var cert = matricula.Certificado;

        return new CertificadoDto
        {
            NomeAluno = cert.NomeAluno,
            NomeCurso = cert.NomeCurso,
            CargaHorariaCurso = cert.CargaHorariaCurso,
            DataConclusao = cert.DataConclusao,
            DataEmissao = cert.DataEmissao
        };
    }
}

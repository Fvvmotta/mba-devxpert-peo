using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MBA_DevXpert_PEO.Alunos.Application.DTOs
{

    public class AlunoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public List<MatriculaCompletaDto> Matriculas { get; set; } = new();
    }
    public class AlunoComMatriculasDto
    {
        public Guid AlunoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<MatriculaDto> Matriculas { get; set; } = new();
    }

    public class MatriculaComCursoDto
    {
        public Guid MatriculaId { get; set; }
        public Guid CursoId { get; set; }
        public DateTime DataMatricula { get; set; }
    }
    
    public class CriarMatriculaDto
    {
        [Required(ErrorMessage = "O ID do curso é obrigatório.")]
        public Guid CursoId { get; set; }
    }
    public class AtualizarAlunoDto
    {
        [Required(ErrorMessage = "O ID do aluno é obrigatório.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail está em formato inválido.")]
        public string Email { get; set; }
    }
    public class CriarAlunoDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail está em formato inválido.")]
        public string Email { get; set; }
    }
    public class MatriculaCompletaDto
    {
        public Guid Id { get; set; }
        public Guid CursoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataMatricula { get; set; }
        public string Status { get; set; }

        public HistoricoAprendizadoDto Historico { get; set; }
    }
    public class HistoricoAprendizadoDto
    {
        public int TotalAulas { get; set; }
        public int AulasConcluidas { get; set; }
        public bool TodasAulasConcluidas { get; set; }
    }
    public class MatriculaDto
    {
        public Guid Id { get; set; }
        public Guid CursoId { get; set; }
        public Guid AlunoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataMatricula { get; set; }
        public string Status { get; set; }
        public CertificadoDto? Certificado { get; set; }
    }

    public class CertificadoDto
    {
        public string NomeAluno { get; set; }
        public string NomeCurso { get; set; }
        public int CargaHorariaCurso { get; set; }
        public DateTime DataConclusao { get; set; }
        public DateTime DataEmissao { get; set; }
    }

    public class AlunoCertificadoDto
    {
        public string Nome { get; set; }
        public DateTime DataEmissao { get; set; }
    }

}

using System;
using System.ComponentModel.DataAnnotations;

namespace MBA_DevXpert_PEO.GestaoDeAlunos.Application.DTOs
{
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
    public class MatriculaDto
    {
        public Guid Id { get; set; }
        public Guid CursoId { get; set; }
        public DateTime DataMatricula { get; set; }
        public string Status { get; set; }
        public CertificadoDto? Certificado { get; set; }
    }

    public class CertificadoDto
    {
        public Guid Id { get; set; }
        public DateTime DataEmissao { get; set; }
    }

}

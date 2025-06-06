using System.ComponentModel.DataAnnotations;

namespace MBA_DevXpert_PEO.Conteudos.Application.DTOs
{
    public class AdicionarCursoDto
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Autor { get; set; }

        [Range(1, 500)]
        public int CargaHoraria { get; set; }

        public string DescricaoConteudoProgramatico { get; set; }
    }

    public class AtualizarCursoDto : AdicionarCursoDto
    {
        [Required]
        public Guid Id { get; set; }
    }

    public class AdicionarAulaDto
    {
        [Required]
        public Guid CursoId { get; set; }

        [Required]
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        [Required]
        public string MaterialUrl { get; set; }
    }

    public class CursoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Autor { get; set; }
        public int CargaHoraria { get; set; }
        public List<AulaDto> Aulas { get; set; } = new();
    }

    public class AulaDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? MaterialUrl { get; set; }
    }
    public class CursoResumoDto
    {
        public Guid CursoId { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public int TotalAulas { get; set; }
    }
}

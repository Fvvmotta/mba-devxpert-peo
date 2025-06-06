using System.ComponentModel.DataAnnotations;

namespace MBA_DevXpert_PEO.Conteudos.Application.DTOs
{
    public class CursoInputDTO
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Autor { get; set; }

        [Range(1, 500)]
        public int CargaHoraria { get; set; }

        public string DescricaoConteudoProgramatico { get; set; }
    }

    public class UpdateCursoInputDTO : CursoInputDTO
    {
        [Required]
        public Guid Id { get; set; }
    }

    public class AdicionarAulaInputDTO
    {
        [Required]
        public Guid CursoId { get; set; }

        [Required]
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        [Required]
        public string MaterialUrl { get; set; }
    }

    public class CursoDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Autor { get; set; }
        public int CargaHoraria { get; set; }
        public List<AulaDTO> Aulas { get; set; } = new();
    }

    public class AulaDTO
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

namespace MBA_DevXpert_PEO.Core.DomainObjects.DTO
{
    public class PagamentoDto
    {
        public Guid Id { get; set; }
        public Guid MatriculaId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public string Status { get; set; }
    }
}
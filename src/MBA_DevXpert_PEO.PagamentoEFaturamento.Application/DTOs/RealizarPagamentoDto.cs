namespace MBA_DevXpert_PEO.PagamentoEFaturamento.Application.DTOs
{
    public class RealizarPagamentoDto
    {
        public Guid MatriculaId { get; set; }
        public decimal Valor { get; set; }

        public string NomeTitular { get; set; }
        public string NumeroCartao { get; set; }
        public string Vencimento { get; set; } // MM/AA
        public string CVV { get; set; }
    }
}

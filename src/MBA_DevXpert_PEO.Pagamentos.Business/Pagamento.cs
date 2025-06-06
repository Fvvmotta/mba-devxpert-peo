using MBA_DevXpert_PEO.Core.DomainObjects;

namespace MBA_DevXpert_PEO.Pagamentos.Business
{
    public class Pagamento : Entity, IAggregateRoot
    {
        public Guid MatriculaId { get; set; }
        public string Status { get; set; }
        public decimal Valor { get; set; }

        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string ExpiracaoCartao { get; set; }
        public string CvvCartao { get; set; }

        // EF. Rel.
        public Transacao Transacao { get; set; }
    }
}

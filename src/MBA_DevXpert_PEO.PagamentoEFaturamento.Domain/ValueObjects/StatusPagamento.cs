using MBA_DevXpert_PEO.Core.DomainObjects;

namespace MBA_DevXpert_PEO.PagamentoEFaturamento.Domain.ValueObjects
{
    public class StatusPagamento : ValueObject
    {
        public StatusPagamentoEnum Valor { get; }

        protected StatusPagamento() { }

        private StatusPagamento(StatusPagamentoEnum valor)
        {
            Valor = valor;
        }

        public static StatusPagamento Pendente => new(StatusPagamentoEnum.Pendente);
        public static StatusPagamento Aprovado => new(StatusPagamentoEnum.Aprovado);
        public static StatusPagamento Recusado => new(StatusPagamentoEnum.Recusado);

        public bool EhAprovado => Valor == StatusPagamentoEnum.Aprovado;
        public bool EhRecusado => Valor == StatusPagamentoEnum.Recusado;
        public bool EhPendente => Valor == StatusPagamentoEnum.Pendente;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }

        public override string ToString() => Valor.ToString();
    }

    public enum StatusPagamentoEnum
    {
        Pendente = 0,
        Aprovado = 1,
        Recusado = 2
    }
}

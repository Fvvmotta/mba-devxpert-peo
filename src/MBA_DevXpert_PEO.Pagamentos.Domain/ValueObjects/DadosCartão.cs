using MBA_DevXpert_PEO.Core.DomainObjects;

namespace MBA_DevXpert_PEO.Pagamentos.Domain.ValueObjects
{
    public class DadosCartao : ValueObject
    {
        public string NomeTitular { get; }
        public string NumeroCartao { get; }
        public string Vencimento { get; } // MM/AA
        public string CVV { get; }

        protected DadosCartao() { }

        public DadosCartao(string nomeTitular, string numeroCartao, string vencimento, string cvv)
        {
            NomeTitular = nomeTitular;
            NumeroCartao = numeroCartao;
            Vencimento = vencimento;
            CVV = cvv;

            Validar();
        }

        private void Validar()
        {
            Validacoes.ValidarSeVazio(NomeTitular, "Nome do titular do cartão é obrigatório.");
            Validacoes.ValidarSeVazio(NumeroCartao, "Número do cartão é obrigatório.");
            Validacoes.ValidarTamanho(NumeroCartao, 12, 19, "Número do cartão deve ter entre 12 e 19 dígitos.");

            Validacoes.ValidarSeVazio(Vencimento, "Vencimento é obrigatório.");
            Validacoes.ValidarSeDiferente(@"^(0[1-9]|1[0-2])\/\d{2}$", Vencimento, "Formato do vencimento deve ser MM/AA.");

            Validacoes.ValidarSeVazio(CVV, "CVV é obrigatório.");
            Validacoes.ValidarTamanho(CVV, 3, 4, "CVV deve ter 3 ou 4 dígitos.");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return NumeroCartao;
            yield return Vencimento;
            yield return CVV;
        }
    }
}

using MBA_DevXpert_PEO.Core.DomainObjects;

namespace MBA_DevXpert_PEO.GestaoDeConteudo.Domain.ValueObjects
{
    public class ConteudoProgramatico : ValueObject
    {
        public string Descricao { get; }

        protected ConteudoProgramatico() { }

        private ConteudoProgramatico(string descricao)
        {
            Validacoes.ValidarSeVazio(descricao, "Descrição do conteúdo programático não pode ser vazia.");
            Validacoes.ValidarTamanho(descricao, 500, "Descrição do conteúdo programático deve ter no máximo 500 caracteres.");
            Descricao = descricao.Trim();
        }

        public static ConteudoProgramatico Criar(string descricao)
        {
            return new ConteudoProgramatico(descricao);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Descricao;
        }
    }
}

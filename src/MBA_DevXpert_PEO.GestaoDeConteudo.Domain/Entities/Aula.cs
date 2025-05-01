using MBA_DevXpert_PEO.Core.DomainObjects;

namespace MBA_DevXpert_PEO.GestaoDeConteudo.Domain.Entities
{
    public class Aula : Entity
    {
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public string MaterialUrl { get; private set; }

        protected Aula() { }

        public Aula(string titulo, string descricao, string materialUrl = null)
        {
            Id = Guid.NewGuid();
            SetTitulo(titulo);
            SetDescricao(descricao);
            MaterialUrl = materialUrl;
        }

        private void SetTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título da aula não pode ser vazio.");

            Titulo = titulo;
        }

        private void SetDescricao(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição da aula não pode ser vazia.");

            Descricao = descricao;
        }
    }
}
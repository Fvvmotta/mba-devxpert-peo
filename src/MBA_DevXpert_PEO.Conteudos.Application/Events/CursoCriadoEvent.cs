using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Events
{
    public class CursoCriadoEvent : Event
    {
        public Guid CursoId { get; }
        public string Nome { get; }
        public string Autor { get; }

        public CursoCriadoEvent(Guid cursoId, string nome, string autor)
        {
            AggregateId = cursoId;
            CursoId = cursoId;
            Nome = nome;
            Autor = autor;
        }
    }
}

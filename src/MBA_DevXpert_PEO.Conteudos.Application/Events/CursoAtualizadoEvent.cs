using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Events
{
    public class CursoAtualizadoEvent : Event
    {
        public Guid CursoId { get; }
        public string Nome { get; }

        public CursoAtualizadoEvent(Guid cursoId, string nome)
        {
            AggregateId = cursoId;
            CursoId = cursoId;
            Nome = nome;
        }
    }
}

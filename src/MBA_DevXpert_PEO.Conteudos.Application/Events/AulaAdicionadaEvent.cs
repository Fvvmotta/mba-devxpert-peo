using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Events
{
    public class AulaAdicionadaEvent : Event
    {
        public Guid CursoId { get; }
        public Guid AulaId { get; }
        public string Titulo { get; }

        public AulaAdicionadaEvent(Guid cursoId, Guid aulaId, string titulo)
        {
            AggregateId = cursoId;
            CursoId = cursoId;
            AulaId = aulaId;
            Titulo = titulo;
        }
    }
}

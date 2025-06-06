using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Alunos.Application.Events
{
    public class AlunoAtualizadoEvent : Event
    {
        public Guid AlunoId { get; }

        public AlunoAtualizadoEvent(Guid alunoId)
        {
            AlunoId = alunoId;
            AggregateId = alunoId;
        }
    }

}

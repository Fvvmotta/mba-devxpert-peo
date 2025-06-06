using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Alunos.Application.Events
{
    public class AlunoCriadoEvent : Event
    {
        public Guid AlunoId { get; }
        public string Nome { get; }
        public string Email { get; }

        public AlunoCriadoEvent(Guid alunoId, string nome, string email)
        {
            AlunoId = alunoId;
            Nome = nome;
            Email = email;
            AggregateId = alunoId;
        }
    }
}

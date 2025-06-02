using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Commands
{
    public class DeleteCursoCommand : Command<bool>
    {
        public Guid Id { get; set; }

        public DeleteCursoCommand(Guid id)
        {
            AggregateId = id;
            Id = id;
        }

        public override bool EhValido()
        {
            return true;
        }
    }
}

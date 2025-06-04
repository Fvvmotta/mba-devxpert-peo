using MBA_DevXpert_PEO.Core.Messages;
using System;

namespace MBA_DevXpert_PEO.Conteudos.Application.Commands
{
    public class DeleteAulaCursoCommand : Command<bool>
    {
        public Guid CursoId { get; }
        public Guid AulaId { get; }

        public DeleteAulaCursoCommand(Guid cursoId, Guid aulaId)
        {
            CursoId = cursoId;
            AulaId = aulaId;
        }

        public override bool EhValido()
        {
            return CursoId != Guid.Empty && AulaId != Guid.Empty;
        }
    }
}

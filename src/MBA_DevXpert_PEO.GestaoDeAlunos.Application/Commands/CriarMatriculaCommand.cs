using MBA_DevXpert_PEO.Core.Messages;
using System;

namespace MBA_DevXpert_PEO.GestaoDeAlunos.Application.Commands
{
    public class CriarMatriculaCommand : Command<bool>
    {
        public Guid AlunoId { get; init; }
        public Guid CursoId { get; init; }

        public CriarMatriculaCommand(Guid alunoId, Guid cursoId)
        {
            AlunoId = alunoId;
            CursoId = cursoId;
        }

        public override bool EhValido()
        {
            return AlunoId != Guid.Empty && CursoId != Guid.Empty;
        }
    }
}

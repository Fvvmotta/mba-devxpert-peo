using MBA_DevXpert_PEO.Core.Messages;
using System;

namespace MBA_DevXpert_PEO.Alunos.Application.Commands
{
    public class FinalizarCursoCommand : Command<bool>
    {
        public Guid AlunoId { get; init; }
        public Guid MatriculaId { get; init; }

        public FinalizarCursoCommand(Guid alunoId, Guid matriculaId)
        {
            AlunoId = alunoId;
            MatriculaId = matriculaId;
        }

        public override bool EhValido()
        {
            return AlunoId != Guid.Empty && MatriculaId != Guid.Empty;
        }
    }
}

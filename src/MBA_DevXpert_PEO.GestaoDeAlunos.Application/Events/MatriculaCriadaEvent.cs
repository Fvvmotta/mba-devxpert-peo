using MBA_DevXpert_PEO.Core.Messages;
using System;

namespace MBA_DevXpert_PEO.GestaoDeAlunos.Application.Events
{
    public class MatriculaCriadaEvent : Event
    {
        public Guid AlunoId { get; }
        public Guid CursoId { get; }
        public Guid MatriculaId { get; }

        public MatriculaCriadaEvent(Guid alunoId, Guid cursoId, Guid matriculaId)
        {
            AlunoId = alunoId;
            CursoId = cursoId;
            MatriculaId = matriculaId;
        }
    }
}

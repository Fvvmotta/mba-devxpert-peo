using MBA_DevXpert_PEO.Core.Messages;
using System;

namespace MBA_DevXpert_PEO.Alunos.Application.Events
{
    public class CursoFinalizadoEvent : Event
    {
        public Guid AlunoId { get; }
        public Guid MatriculaId { get; }
        public Guid CertificadoId { get; }

        public CursoFinalizadoEvent(Guid alunoId, Guid matriculaId, Guid certificadoId)
        {
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            CertificadoId = certificadoId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Alunos.Application.Events
{
    public class AulaFinalizadaEvent : Event
    {
        public Guid AlunoId { get; }
        public Guid MatriculaId { get; }

        public AulaFinalizadaEvent(Guid alunoId, Guid matriculaId)
        {
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            AggregateId = alunoId;
        }
    }

}

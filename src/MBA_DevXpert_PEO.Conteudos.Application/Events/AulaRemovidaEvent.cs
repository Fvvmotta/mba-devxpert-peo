using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Events
{
    public class AulaRemovidaEvent : Event
    {
        public Guid CursoId { get; }
        public Guid AulaId { get; }

        public AulaRemovidaEvent(Guid cursoId, Guid aulaId)
        {
            AggregateId = cursoId;
            CursoId = cursoId;
            AulaId = aulaId;
        }
    }
}

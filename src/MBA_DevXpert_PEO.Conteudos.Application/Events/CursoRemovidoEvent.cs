using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Events
{
    public class CursoRemovidoEvent : Event
    {
        public Guid CursoId { get; }

        public CursoRemovidoEvent(Guid cursoId)
        {
            AggregateId = cursoId;
            CursoId = cursoId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Core.Messages;

namespace Alunos.Commands
{
    public class FinalizarAulaCommand : Command<bool>
    {
        public Guid AlunoId { get; }
        public Guid MatriculaId { get; }
        public int TotalAulasCurso { get; }

        public FinalizarAulaCommand(Guid alunoId, Guid matriculaId, int totalAulasCurso)
        {
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            TotalAulasCurso = totalAulasCurso;
        }

        public override bool EhValido() => AlunoId != Guid.Empty && MatriculaId != Guid.Empty && TotalAulasCurso > 0;
    }

}

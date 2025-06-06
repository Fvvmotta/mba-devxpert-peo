using MBA_DevXpert_PEO.Core.Messages;
using System;

namespace MBA_DevXpert_PEO.Alunos.Application.Commands
{
    public class CriarMatriculaCommand : Command<bool>
    {
        public Guid AlunoId { get; init; }
        public Guid CursoId { get; init; }
        public decimal Valor { get; private set; }

        public int TotalAulas { get; private set; }

        public CriarMatriculaCommand(Guid alunoId, Guid cursoId, decimal valor, int totalAulas)
        {
            AggregateId = alunoId;
            AlunoId = alunoId;
            CursoId = cursoId;
            Valor = valor;
            TotalAulas = totalAulas;
        }

        public override bool EhValido()
        {
            return AlunoId != Guid.Empty && CursoId != Guid.Empty;
        }
    }
}

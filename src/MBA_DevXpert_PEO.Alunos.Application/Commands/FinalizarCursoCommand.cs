using MBA_DevXpert_PEO.Core.Messages;
using System;

namespace MBA_DevXpert_PEO.Alunos.Application.Commands
{
    public class FinalizarCursoCommand : Command<bool>
    {
        public Guid AlunoId { get; init; }
        public Guid MatriculaId { get; init; }
        public string AlunoNome { get; set; }
        public string CursoNome { get; set; }
        public int CargaHoraria { get; set; }

        public FinalizarCursoCommand(Guid alunoId, Guid matriculaId, string alunoNome, string cursoNome, int cargaHoraria)
        {
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            AlunoNome = alunoNome;
            CursoNome = cursoNome;
            CargaHoraria = cargaHoraria;
        }

        public override bool EhValido()
        {
            return AlunoId != Guid.Empty && MatriculaId != Guid.Empty;
        }
    }
}

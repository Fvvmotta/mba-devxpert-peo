using FluentValidation;
using FluentValidation.Results;
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

        public override bool EhValido()
        {
            ValidationResult = new FinalizarAulaValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class FinalizarAulaValidation : AbstractValidator<FinalizarAulaCommand>
    {
        public const string AlunoIdErroMsg = "O ID do aluno é obrigatório.";
        public const string MatriculaIdErroMsg = "O ID da matrícula é obrigatório.";
        public const string TotalAulasErroMsg = "O total de aulas do curso deve ser maior que zero.";

        public FinalizarAulaValidation()
        {
            RuleFor(c => c.AlunoId)
                .NotEqual(Guid.Empty).WithMessage(AlunoIdErroMsg);

            RuleFor(c => c.MatriculaId)
                .NotEqual(Guid.Empty).WithMessage(MatriculaIdErroMsg);

            RuleFor(c => c.TotalAulasCurso)
                .GreaterThan(0).WithMessage(TotalAulasErroMsg);
        }
    }

}

using FluentValidation;
using FluentValidation.Results;
using MBA_DevXpert_PEO.Alunos.Application.Commands;
using MBA_DevXpert_PEO.Core.Messages;

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
            ValidationResult = new CriarMatriculaValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }


    public class CriarMatriculaValidation : AbstractValidator<CriarMatriculaCommand>
    {
        public CriarMatriculaValidation()
        {
            RuleFor(c => c.AlunoId)
                .NotEqual(Guid.Empty).WithMessage("O ID do aluno é obrigatório.");

            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty).WithMessage("O ID do curso é obrigatório.");

            RuleFor(c => c.Valor)
                .GreaterThan(0).WithMessage("O valor do curso deve ser maior que zero.");

            RuleFor(c => c.TotalAulas)
                .GreaterThan(0).WithMessage("O total de aulas deve ser maior que zero.");
        }
    }

}


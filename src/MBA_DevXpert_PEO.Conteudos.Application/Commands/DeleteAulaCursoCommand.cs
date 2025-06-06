using FluentValidation;
using FluentValidation.Results;
using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Commands
{
    public class DeleteAulaCursoCommand : Command<bool>
    {
        public Guid CursoId { get; }
        public Guid AulaId { get; }

        public DeleteAulaCursoCommand(Guid cursoId, Guid aulaId)
        {
            CursoId = cursoId;
            AulaId = aulaId;
        }

        public override bool EhValido()
        {
            ValidationResult = new DeleteAulaCursoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class DeleteAulaCursoValidation : AbstractValidator<DeleteAulaCursoCommand>
    {
        public DeleteAulaCursoValidation()
        {
            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty).WithMessage("O ID do curso é obrigatório.");

            RuleFor(c => c.AulaId)
                .NotEqual(Guid.Empty).WithMessage("O ID da aula é obrigatório.");
        }
    }
}

using FluentValidation;
using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Commands
{
    public class ExcluirAulaCursoCommand : Command<bool>
    {
        public Guid CursoId { get; }
        public Guid AulaId { get; }

        public ExcluirAulaCursoCommand(Guid cursoId, Guid aulaId)
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

    public class DeleteAulaCursoValidation : AbstractValidator<ExcluirAulaCursoCommand>
    {
        public const string CursoIdObrigatorioMsg = "O ID do curso é obrigatório.";
        public const string AulaIdObrigatorioMsg = "O ID da aula é obrigatório.";

        public DeleteAulaCursoValidation()
        {
            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty)
                .WithMessage(CursoIdObrigatorioMsg);

            RuleFor(c => c.AulaId)
                .NotEqual(Guid.Empty)
                .WithMessage(AulaIdObrigatorioMsg);
        }
    }
}

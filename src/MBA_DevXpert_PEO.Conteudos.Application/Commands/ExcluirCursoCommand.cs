using FluentValidation;
using FluentValidation.Results;
using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Commands
{
    public class ExcluirCursoCommand : Command<bool>
    {
        public Guid Id { get; set; }

        public ExcluirCursoCommand(Guid id)
        {
            AggregateId = id;
            Id = id;
        }

        public override bool EhValido()
        {
            ValidationResult = new DeleteCursoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class DeleteCursoValidation : AbstractValidator<ExcluirCursoCommand>
    {
        public const string CursoIdObrigatorioMsg = "O ID do curso é obrigatório.";
        public DeleteCursoValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage(CursoIdObrigatorioMsg);
        }
    }
}

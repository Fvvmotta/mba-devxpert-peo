using FluentValidation;
using FluentValidation.Results;
using MBA_DevXpert_PEO.Core.Messages;
namespace MBA_DevXpert_PEO.Alunos.Application.Commands
{
    public class AtualizarAlunoCommand : Command<bool>
    {
        public Guid AlunoId { get; init; }
        public string Nome { get; init; }
        public string Email { get; init; }

        public AtualizarAlunoCommand(Guid alunoId, string nome, string email)
        {
            AlunoId = alunoId;
            Nome = nome;
            Email = email;
        }

        public override bool EhValido()
        {
            ValidationResult = new AtualizarAlunoValidation().Validate(this);
            return ValidationResult.IsValid;
        }


    }
    public class AtualizarAlunoValidation : AbstractValidator<AtualizarAlunoCommand>
    {
        public AtualizarAlunoValidation()
        {
            RuleFor(c => c.AlunoId)
                .NotEqual(Guid.Empty).WithMessage("O ID do aluno é obrigatório.");

            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome do aluno é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do aluno deve ter no máximo 100 caracteres.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("O e-mail do aluno é obrigatório.")
                .EmailAddress().WithMessage("O e-mail informado não é válido.");
        }
    }

}


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
        public static string IdAlunoErroMsg => "O ID do aluno é obrigatório.";
        public static string NomeErroMsg => "O nome do aluno é obrigatório.";
        public static string NomeMaxErroMsg => "O nome do aluno deve ter no máximo 100 caracteres.";
        public static string EmailErroMsg => "O e-mail do aluno é obrigatório.";
        public static string EmailInvalidoErroMsg => "O e-mail informado não é válido.";

        public AtualizarAlunoValidation()
        {
            RuleFor(c => c.AlunoId)
                .NotEqual(Guid.Empty).WithMessage(IdAlunoErroMsg);

            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage(NomeErroMsg)
                .MaximumLength(100).WithMessage(NomeMaxErroMsg);

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage(EmailErroMsg)
                .EmailAddress().WithMessage(EmailInvalidoErroMsg);
        }
    }
}


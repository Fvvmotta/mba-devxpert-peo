using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MBA_DevXpert_PEO.Core.Messages;

namespace Alunos.Commands
{
    public class CriarAlunoCommand : Command<bool>
    {
        public Guid AlunoId { get; }
        public string Nome { get; }
        public string Email { get; }

        public CriarAlunoCommand(Guid alunoId, string nome, string email)
        {
            AlunoId = alunoId;
            Nome = nome;
            Email = email;
            AggregateId = alunoId;
        }

        public override bool EhValido()
        {
            ValidationResult = new CriarAlunoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class CriarAlunoValidation : AbstractValidator<CriarAlunoCommand>
    {
        public const string IdObrigatorioMsg = "O ID do aluno é obrigatório.";
        public const string NomeObrigatorioMsg = "O nome é obrigatório.";
        public const string EmailObrigatorioMsg = "O e-mail é obrigatório.";
        public const string EmailInvalidoMsg = "E-mail inválido.";

        public CriarAlunoValidation()
        {
            RuleFor(c => c.AlunoId)
                .NotEqual(Guid.Empty).WithMessage(IdObrigatorioMsg);

            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage(NomeObrigatorioMsg);

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage(EmailObrigatorioMsg)
                .EmailAddress().WithMessage(EmailInvalidoMsg);
        }
    }

}

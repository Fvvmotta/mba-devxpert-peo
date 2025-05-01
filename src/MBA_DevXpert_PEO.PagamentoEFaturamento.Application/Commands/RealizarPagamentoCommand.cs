using FluentValidation;
using FluentValidation.Results;
using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.PagamentoEFaturamento.Application.Commands
{
    public class RealizarPagamentoCommand : Command<bool>
    {
        public Guid MatriculaId { get; init; }
        public decimal Valor { get; init; }

        public string NomeTitular { get; init; } = string.Empty;
        public string NumeroCartao { get; init; } = string.Empty;
        public string Vencimento { get; init; } = string.Empty; // MM/AA
        public string CVV { get; init; } = string.Empty;

        public override bool EhValido()
        {
            ValidationResult = new RealizarPagamentoCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }

        private class RealizarPagamentoCommandValidator : AbstractValidator<RealizarPagamentoCommand>
        {
            public RealizarPagamentoCommandValidator()
            {
                RuleFor(c => c.MatriculaId)
                    .NotEqual(Guid.Empty).WithMessage("ID da matrícula é obrigatório.");

                RuleFor(c => c.Valor)
                    .GreaterThan(0).WithMessage("Valor do pagamento deve ser maior que zero.");

                RuleFor(c => c.NomeTitular)
                    .NotEmpty().WithMessage("Nome do titular do cartão é obrigatório.");

                RuleFor(c => c.NumeroCartao)
                    .CreditCard().WithMessage("Número do cartão inválido.");

                RuleFor(c => c.Vencimento)
                    .Matches(@"^(0[1-9]|1[0-2])\/\d{2}$").WithMessage("Formato do vencimento deve ser MM/AA.");

                RuleFor(c => c.CVV)
                    .Length(3, 4).WithMessage("CVV deve ter 3 ou 4 dígitos.");
            }
        }
    }
}

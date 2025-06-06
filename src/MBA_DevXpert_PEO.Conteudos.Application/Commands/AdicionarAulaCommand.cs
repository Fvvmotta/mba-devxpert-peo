using FluentValidation;
using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Commands
{
    public class AdicionarAulaCommand : Command<bool>
    {
        public Guid CursoId { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public string MaterialUrl { get; private set; }

        public AdicionarAulaCommand(Guid cursoId, string titulo, string descricao, string materialUrl = null)
        {
            CursoId = cursoId;
            Titulo = titulo;
            Descricao = descricao;
            MaterialUrl = materialUrl;
        }

        public override bool EhValido()
        {
            ValidationResult = new AdicionarAulaValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class AdicionarAulaValidation : AbstractValidator<AdicionarAulaCommand>
    {
        public AdicionarAulaValidation()
        {
            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty).WithMessage("O ID do curso é obrigatório.");

            RuleFor(c => c.Titulo)
                .NotEmpty().WithMessage("O título da aula é obrigatório.")
                .MaximumLength(100).WithMessage("O título deve ter no máximo 100 caracteres.");

            RuleFor(c => c.Descricao)
                .NotEmpty().WithMessage("A descrição da aula é obrigatória.");

            RuleFor(c => c.MaterialUrl)
                .NotEmpty().WithMessage("A URL do material é obrigatória.")
                .MaximumLength(200).WithMessage("A URL do material deve ter no máximo 200 caracteres.");
        }
    }

}

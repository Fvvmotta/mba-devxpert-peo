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
        public const string CursoIdObrigatorioMsg = "O ID do curso é obrigatório.";
        public const string TituloObrigatorioMsg = "O título da aula é obrigatório.";
        public const string TituloMaximoMsg = "O título deve ter no máximo 100 caracteres.";
        public const string DescricaoObrigatoriaMsg = "A descrição da aula é obrigatória.";
        public const string UrlObrigatoriaMsg = "A URL do material é obrigatória.";
        public const string UrlMaximoMsg = "A URL do material deve ter no máximo 200 caracteres.";

        public AdicionarAulaValidation()
        {
            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty)
                .WithMessage(CursoIdObrigatorioMsg);

            RuleFor(c => c.Titulo)
                .NotEmpty().WithMessage(TituloObrigatorioMsg)
                .MaximumLength(100).WithMessage(TituloMaximoMsg);

            RuleFor(c => c.Descricao)
                .NotEmpty().WithMessage(DescricaoObrigatoriaMsg);

            RuleFor(c => c.MaterialUrl)
                .NotEmpty().WithMessage(UrlObrigatoriaMsg)
                .MaximumLength(200).WithMessage(UrlMaximoMsg);
        }
    }
}

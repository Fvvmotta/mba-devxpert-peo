using FluentValidation;
using FluentValidation.Results;
using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Commands
{
    public class CriarCursoCommand : Command<Guid?>
    {
        public string Nome { get; }
        public string Autor { get; }
        public int CargaHoraria { get; }
        public string DescricaoConteudoProgramatico { get; }

        public CriarCursoCommand(string nome, string autor, int cargaHoraria, string descricaoConteudoProgramatico)
        {
            Nome = nome;
            Autor = autor;
            CargaHoraria = cargaHoraria;
            DescricaoConteudoProgramatico = descricaoConteudoProgramatico;
        }

        public override bool EhValido()
        {
            ValidationResult = new CriarCursoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class CriarCursoValidation : AbstractValidator<CriarCursoCommand>
    {
        public CriarCursoValidation()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome do curso é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do curso deve ter no máximo 100 caracteres.");

            RuleFor(c => c.Autor)
                .NotEmpty().WithMessage("O nome do autor é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do autor deve ter no máximo 100 caracteres.");

            RuleFor(c => c.CargaHoraria)
                .GreaterThan(0).WithMessage("A carga horária deve ser maior que zero.");

            RuleFor(c => c.DescricaoConteudoProgramatico)
                .NotEmpty().WithMessage("A descrição do conteúdo programático é obrigatória.")
                .MaximumLength(1000).WithMessage("A descrição do conteúdo deve ter no máximo 1000 caracteres.");
        }
    }
}

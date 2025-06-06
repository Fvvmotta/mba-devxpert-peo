using FluentValidation;
using FluentValidation.Results;
using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Commands
{
    public class UpdateCursoCommand : Command<bool>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int CargaHoraria { get; set; }
        public string Autor { get; set; }
        public string DescricaoConteudoProgramatico { get; set; }

        public UpdateCursoCommand(Guid id, string nome, string autor, int cargaHoraria, string descricaoConteudoProgramatico)
        {
            AggregateId = id;
            Id = id;
            Nome = nome;
            Autor = autor;
            CargaHoraria = cargaHoraria;
            DescricaoConteudoProgramatico = descricaoConteudoProgramatico;
        }

        public override bool EhValido()
        {
            ValidationResult = new UpdateCursoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class UpdateCursoValidation : AbstractValidator<UpdateCursoCommand>
    {
        public UpdateCursoValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage("O ID do curso é obrigatório.");

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
                .MaximumLength(1000).WithMessage("A descrição deve ter no máximo 1000 caracteres.");
        }
    }
}

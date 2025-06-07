using FluentValidation;
using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Commands
{
    public class AtualizarCursoCommand : Command<bool>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int CargaHoraria { get; set; }
        public string Autor { get; set; }
        public string DescricaoConteudoProgramatico { get; set; }

        public AtualizarCursoCommand(Guid id, string nome, string autor, int cargaHoraria, string descricaoConteudoProgramatico)
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

    public class UpdateCursoValidation : AbstractValidator<AtualizarCursoCommand>
    {
        public const string IdObrigatorioMsg = "O ID do curso é obrigatório.";
        public const string NomeObrigatorioMsg = "O nome do curso é obrigatório.";
        public const string NomeMaximoMsg = "O nome do curso deve ter no máximo 100 caracteres.";
        public const string AutorObrigatorioMsg = "O nome do autor é obrigatório.";
        public const string AutorMaximoMsg = "O nome do autor deve ter no máximo 100 caracteres.";
        public const string CargaHorariaInvalidaMsg = "A carga horária deve ser maior que zero.";
        public const string DescricaoObrigatoriaMsg = "A descrição do conteúdo programático é obrigatória.";
        public const string DescricaoMaximoMsg = "A descrição do conteúdo programático deve ter no máximo 1000 caracteres.";

        public UpdateCursoValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage(IdObrigatorioMsg);

            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage(NomeObrigatorioMsg)
                .MaximumLength(100).WithMessage(NomeMaximoMsg);

            RuleFor(c => c.Autor)
                .NotEmpty().WithMessage(AutorObrigatorioMsg)
                .MaximumLength(100).WithMessage(AutorMaximoMsg);

            RuleFor(c => c.CargaHoraria)
                .GreaterThan(0).WithMessage(CargaHorariaInvalidaMsg);

            RuleFor(c => c.DescricaoConteudoProgramatico)
                .NotEmpty().WithMessage(DescricaoObrigatoriaMsg)
                .MaximumLength(1000).WithMessage(DescricaoMaximoMsg);
        }
    }
}

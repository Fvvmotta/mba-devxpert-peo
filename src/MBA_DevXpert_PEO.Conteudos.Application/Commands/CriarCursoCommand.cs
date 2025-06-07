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
        public const string NomeObrigatorioMsg = "O nome do curso é obrigatório.";
        public const string NomeTamanhoMaxMsg = "O nome do curso deve ter no máximo 100 caracteres.";
        public const string AutorObrigatorioMsg = "O nome do autor é obrigatório.";
        public const string AutorTamanhoMaxMsg = "O nome do autor deve ter no máximo 100 caracteres.";
        public const string CargaHorariaInvalidaMsg = "A carga horária deve ser maior que zero.";
        public const string DescricaoObrigatoriaMsg = "A descrição do conteúdo programático é obrigatória.";
        public const string DescricaoTamanhoMaxMsg = "A descrição do conteúdo programático deve ter no máximo 1000 caracteres.";

        public CriarCursoValidation()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage(NomeObrigatorioMsg)
                .MaximumLength(100).WithMessage(NomeTamanhoMaxMsg);

            RuleFor(c => c.Autor)
                .NotEmpty().WithMessage(AutorObrigatorioMsg)
                .MaximumLength(100).WithMessage(AutorTamanhoMaxMsg);

            RuleFor(c => c.CargaHoraria)
                .GreaterThan(0).WithMessage(CargaHorariaInvalidaMsg);

            RuleFor(c => c.DescricaoConteudoProgramatico)
                .NotEmpty().WithMessage(DescricaoObrigatoriaMsg)
                .MaximumLength(1000).WithMessage(DescricaoTamanhoMaxMsg);
        }
    }
}

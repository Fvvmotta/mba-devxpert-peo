using FluentValidation;
using FluentValidation.Results;
using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Alunos.Application.Commands
{
    public class FinalizarCursoCommand : Command<bool>
    {
        public Guid AlunoId { get; init; }
        public Guid MatriculaId { get; init; }
        public string AlunoNome { get; set; }
        public string CursoNome { get; set; }
        public int CargaHoraria { get; set; }

        public FinalizarCursoCommand(Guid alunoId, Guid matriculaId, string alunoNome, string cursoNome, int cargaHoraria)
        {
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            AlunoNome = alunoNome;
            CursoNome = cursoNome;
            CargaHoraria = cargaHoraria;
        }

        public override bool EhValido()
        {
            ValidationResult = new FinalizarCursoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class FinalizarCursoValidation : AbstractValidator<FinalizarCursoCommand>
    {
        public const string AlunoIdErroMsg = "O ID do aluno é obrigatório.";
        public const string MatriculaIdErroMsg = "O ID da matrícula é obrigatório.";
        public const string AlunoNomeErroMsg = "O nome do aluno é obrigatório.";
        public const string AlunoNomeMaxErroMsg = "O nome do aluno deve ter no máximo 100 caracteres.";
        public const string CursoNomeErroMsg = "O nome do curso é obrigatório.";
        public const string CursoNomeMaxErroMsg = "O nome do curso deve ter no máximo 100 caracteres.";
        public const string CargaHorariaErroMsg = "A carga horária do curso deve ser maior que zero.";

        public FinalizarCursoValidation()
        {
            RuleFor(c => c.AlunoId)
                .NotEqual(Guid.Empty).WithMessage(AlunoIdErroMsg);

            RuleFor(c => c.MatriculaId)
                .NotEqual(Guid.Empty).WithMessage(MatriculaIdErroMsg);

            RuleFor(c => c.AlunoNome)
                .NotEmpty().WithMessage(AlunoNomeErroMsg)
                .MaximumLength(100).WithMessage(AlunoNomeMaxErroMsg);

            RuleFor(c => c.CursoNome)
                .NotEmpty().WithMessage(CursoNomeErroMsg)
                .MaximumLength(100).WithMessage(CursoNomeMaxErroMsg);

            RuleFor(c => c.CargaHoraria)
                .GreaterThan(0).WithMessage(CargaHorariaErroMsg);
        }
    }
}

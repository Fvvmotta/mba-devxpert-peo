using MBA_DevXpert_PEO.Core.DomainObjects;

namespace MBA_DevXpert_PEO.Alunos.Domain.Entities
{
    public class Aluno : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }

        private readonly List<Matricula> _matriculas = new();
        public IReadOnlyCollection<Matricula> Matriculas => _matriculas.AsReadOnly();

        protected Aluno() { }

        public Aluno(string nome, string email)
        {
            Id = Guid.NewGuid();
            AtualizarDados(nome, email);
        }

        public void Matricular(Matricula matricula)
        {
            Validacoes.ValidarSeNulo(matricula, "Matrícula inválida.");

            if (_matriculas.Any(m => m.CursoId == matricula.CursoId))
                throw new DomainException("O aluno já está matriculado neste curso.");

            _matriculas.Add(matricula);
        }

        public void ConcluirMatricula(Guid matriculaId, string nomeAluno, string nomeCurso, int cargaHorariaCurso, DateTime dataConclusao)
        {
            var matricula = _matriculas.FirstOrDefault(m => m.Id == matriculaId);
            if (matricula == null)
                throw new DomainException("Matrícula não encontrada.");

            matricula.Concluir(nomeAluno, nomeCurso, cargaHorariaCurso, dataConclusao);
        }

        public void AtualizarDados(string nome, string email)
        {
            Validacoes.ValidarSeVazio(nome, "Nome não pode ser vazio.");
            Validacoes.ValidarSeVazio(email, "Email não pode ser vazio.");

            Nome = nome;
            Email = email;
        }

        public static Aluno CriarComId(Guid id, string nome, string email)
        {
            var aluno = new Aluno(nome, email)
            {
                Id = id
            };
            return aluno;
        }
    }
}

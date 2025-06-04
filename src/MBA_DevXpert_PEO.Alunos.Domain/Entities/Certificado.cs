using MBA_DevXpert_PEO.Core.DomainObjects;

namespace MBA_DevXpert_PEO.Alunos.Domain.Entities
{
    public class Certificado : Entity
    {
        public Guid MatriculaId { get; private set; }
        public string NomeAluno { get; private set; }
        public string NomeCurso { get; private set; }
        public int CargaHorariaCurso { get; private set; }
        public DateTime DataConclusao { get; private set; }
        public DateTime DataEmissao { get; private set; }

        protected Certificado() { }
        public Certificado(Guid matriculaId, string nomeAluno, string nomeCurso, int cargaHoraria, DateTime dataConclusao)
        {
            Id = Guid.NewGuid();
            MatriculaId = matriculaId;
            NomeAluno = nomeAluno;
            NomeCurso = nomeCurso;
            CargaHorariaCurso = cargaHoraria;
            DataConclusao = dataConclusao;
            DataEmissao = DateTime.UtcNow;
        }
    }
}

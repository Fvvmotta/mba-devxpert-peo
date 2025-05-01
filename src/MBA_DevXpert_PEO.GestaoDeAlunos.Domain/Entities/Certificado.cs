using MBA_DevXpert_PEO.Core.DomainObjects;

namespace MBA_DevXpert_PEO.GestaoDeAlunos.Domain.Entities
{
    public class Certificado : Entity
    {
        public Guid MatriculaId { get; private set; }
        public DateTime DataEmissao { get; private set; }

        protected Certificado() { }

        public Certificado(Guid matriculaId)
        {
            Id = Guid.NewGuid();
            MatriculaId = matriculaId;
            DataEmissao = DateTime.UtcNow;
        }
    }
}

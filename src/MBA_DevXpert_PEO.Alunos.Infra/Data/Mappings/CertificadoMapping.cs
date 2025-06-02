using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA_DevXpert_PEO.Alunos.Infra.Mappings
{
    public class CertificadoMapping : IEntityTypeConfiguration<Certificado>
    {
        public void Configure(EntityTypeBuilder<Certificado> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.MatriculaId)
                .IsRequired();

            builder.Property(c => c.DataEmissao)
                .IsRequired();

            builder.ToTable("Certificados");
        }
    }
}

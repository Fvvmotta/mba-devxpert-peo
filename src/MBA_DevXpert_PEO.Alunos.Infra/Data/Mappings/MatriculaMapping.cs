using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA_DevXpert_PEO.Alunos.Infra.Mappings
{
    public class MatriculaMapping : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.HasKey(m => m.Id);
            builder.HasOne<Aluno>()
                .WithMany(a => a.Matriculas)
                .HasForeignKey(m => m.AlunoId);

            builder.Property(m => m.CursoId)
                .IsRequired();

            builder.Property(m => m.DataMatricula)
                .IsRequired();

            builder.Property(m => m.Status)
                .IsRequired();

            // Value Object: HistoricoAprendizado
            builder.OwnsOne(m => m.Historico, h =>
            {
                h.Property(p => p.TotalAulas)
                    .IsRequired()
                    .HasColumnName("TotalAulas");

                h.Property(p => p.AulasConcluidas)
                    .IsRequired()
                    .HasColumnName("AulasConcluidas");
            });

            builder.HasOne(m => m.Certificado)
                .WithOne()
                .HasForeignKey<Certificado>(c => c.MatriculaId);

            builder.ToTable("Matriculas");
        }
    }
}

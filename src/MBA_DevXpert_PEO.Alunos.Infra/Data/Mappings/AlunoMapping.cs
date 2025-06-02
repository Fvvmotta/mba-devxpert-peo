using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA_DevXpert_PEO.Alunos.Infra.Mappings
{
    public class AlunoMapping : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Nome)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(a => a.Email)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.HasMany(a => a.Matriculas)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Alunos");
        }
    }
}

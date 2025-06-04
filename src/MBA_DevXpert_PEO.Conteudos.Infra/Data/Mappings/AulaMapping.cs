using MBA_DevXpert_PEO.Conteudos.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class AulaMapping : IEntityTypeConfiguration<Aula>
{
    public void Configure(EntityTypeBuilder<Aula> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Titulo)
            .IsRequired()
            .HasColumnType("varchar(150)");

        builder.Property(a => a.Descricao)
            .IsRequired()
            .HasColumnType("varchar(500)");

        builder.Property(a => a.MaterialUrl)
            .HasColumnType("varchar(500)");

        builder.Property(a => a.CursoId)
            .IsRequired();

        builder.HasOne(a => a.Curso)
            .WithMany(c => c.Aulas)
            .HasForeignKey(a => a.CursoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Aulas");
    }

}

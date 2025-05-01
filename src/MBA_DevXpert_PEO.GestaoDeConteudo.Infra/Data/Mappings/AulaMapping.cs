using MBA_DevXpert_PEO.GestaoDeConteudo.Domain.Entities;
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

        builder.ToTable("Aulas");
    }
}

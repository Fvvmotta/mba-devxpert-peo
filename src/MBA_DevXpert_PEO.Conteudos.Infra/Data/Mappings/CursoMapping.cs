using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MBA_DevXpert_PEO.Conteudos.Domain.Entities;

namespace MBA_DevXpert_PEO.Services.Conteudos.Infra.Data.Mappings
{
    public class CursoMapping : IEntityTypeConfiguration<Curso>
    {
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(c => c.Autor)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(c => c.CargaHoraria)
                .IsRequired();

            builder.OwnsOne(c => c.ConteudoProgramatico, cb =>
            {
                cb.Property(c => c.Descricao)
                    .HasColumnName("ConteudoProgramatico")
                    .HasColumnType("varchar(1000)")
                    .IsRequired();
            });

            builder.HasMany(c => c.Aulas)
                .WithOne()
                .HasForeignKey("CursoId");

            builder.ToTable("Cursos");
        }
    }
}

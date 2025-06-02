using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MBA_DevXpert_PEO.Pagamentos.Domain.Entities;
using MBA_DevXpert_PEO.Pagamentos.Domain.ValueObjects;

namespace MBA_DevXpert_PEO.Pagamentos.Infra.Data.Mappings
{
    public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.ToTable("Pagamentos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.MatriculaId)
                .IsRequired();

            builder.Property(p => p.Valor)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.DataPagamento)
                .IsRequired();

            builder.OwnsOne(p => p.Cartao, cartao =>
            {
                cartao.Property(c => c.NomeTitular)
                    .HasColumnName("NomeTitular")
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                cartao.Property(c => c.NumeroCartao)
                    .HasColumnName("NumeroCartao")
                    .HasColumnType("varchar(19)")
                    .IsRequired();

                cartao.Property(c => c.Vencimento)
                    .HasColumnName("Vencimento")
                    .HasColumnType("varchar(5)")
                    .IsRequired();

                cartao.Property(c => c.CVV)
                    .HasColumnName("CVV")
                    .HasColumnType("varchar(4)")
                    .IsRequired();
            });

            builder.OwnsOne(p => p.Status, status =>
            {
                status.Property(s => s.Valor)
                    .HasColumnName("StatusPagamento")
                    .IsRequired()
                    .HasConversion<int>(); 
            });
        }
    }
}

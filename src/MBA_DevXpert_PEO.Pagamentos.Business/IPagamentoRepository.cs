using MBA_DevXpert_PEO.Core.Data;

namespace MBA_DevXpert_PEO.Pagamentos.Business
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        void Adicionar(Pagamento pagamento);

        void AdicionarTransacao(Transacao transacao);
    }
}

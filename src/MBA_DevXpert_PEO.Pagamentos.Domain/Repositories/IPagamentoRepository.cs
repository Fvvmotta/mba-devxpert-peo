using MBA_DevXpert_PEO.Core.Data;
using MBA_DevXpert_PEO.Pagamentos.Domain.Entities;

namespace MBA_DevXpert_PEO.Pagamentos.Domain.Repositories
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        Task<IEnumerable<Pagamento>> ObterTodos();
        Task<Pagamento> ObterPorId(Guid id);
        Task<IEnumerable<Pagamento>> ObterPorMatriculas(IEnumerable<Guid> matriculaIds);
        Task<Pagamento> ObterPorMatricula(Guid matriculaId);
        void Adicionar(Pagamento pagamento);
    }
}

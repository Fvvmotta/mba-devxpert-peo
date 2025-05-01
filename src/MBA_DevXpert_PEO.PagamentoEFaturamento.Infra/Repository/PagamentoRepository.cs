using Microsoft.EntityFrameworkCore;
using MBA_DevXpert_PEO.Core.Data;
using MBA_DevXpert_PEO.PagamentoEFaturamento.Domain.Entities;
using MBA_DevXpert_PEO.PagamentoEFaturamento.Domain.Repositories;
using MBA_DevXpert_PEO.PagamentoEFaturamento.Infra.Context;

namespace MBA_DevXpert_PEO.PagamentoEFaturamento.Infra.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly PagamentosContext _context;

        public PagamentoRepository(PagamentosContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Pagamento>> ObterTodos()
        {
            return await _context.Pagamentos.ToListAsync();
        }
        public void Adicionar(Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
        }

        public async Task<Pagamento> ObterPorId(Guid id)
        {
            return await _context.Pagamentos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<IEnumerable<Pagamento>> ObterPorMatriculas(IEnumerable<Guid> matriculaIds)
        {
            return await _context.Pagamentos
                .Where(p => matriculaIds.Contains(p.MatriculaId))
                .ToListAsync();
        }
        public async Task<Pagamento> ObterPorMatricula(Guid matriculaId)
        {
            return await _context.Pagamentos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.MatriculaId == matriculaId);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

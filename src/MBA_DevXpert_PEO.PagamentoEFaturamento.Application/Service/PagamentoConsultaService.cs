using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Core.DomainObjects.DTO;
using MBA_DevXpert_PEO.Core.DomainObjects.Services;
using MBA_DevXpert_PEO.PagamentoEFaturamento.Domain.Repositories;

namespace MBA_DevXpert_PEO.PagamentoEFaturamento.Application.Service
{
    public class PagamentoConsultaService : IPagamentoConsultaService
    {
        private readonly IPagamentoRepository _pagamentoRepository;

        public PagamentoConsultaService(IPagamentoRepository pagamentoRepository)
        {
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task<IEnumerable<PagamentoDto>> ObterPagamentosPorMatriculas(IEnumerable<Guid> matriculaIds)
        {
            var pagamentos = await _pagamentoRepository
                .ObterPorMatriculas(matriculaIds);

            return pagamentos.Select(p => new PagamentoDto
            {
                Id = p.Id,
                MatriculaId = p.MatriculaId,
                Valor = p.Valor,
                DataPagamento = p.DataPagamento,
                Status = p.Status.Valor.ToString()
            });
        }
    }

}

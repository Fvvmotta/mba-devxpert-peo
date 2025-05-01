using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Core.DomainObjects.DTO;

namespace MBA_DevXpert_PEO.Core.DomainObjects.Services
{
    public interface IPagamentoConsultaService
    {
        Task<IEnumerable<PagamentoDto>> ObterPagamentosPorMatriculas(IEnumerable<Guid> matriculaIds);
    }
}

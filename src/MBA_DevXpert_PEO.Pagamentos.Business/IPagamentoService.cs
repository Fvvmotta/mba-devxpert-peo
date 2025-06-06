using MBA_DevXpert_PEO.Core.DomainObjects.DTO;

namespace MBA_DevXpert_PEO.Pagamentos.Business
{
    public interface IPagamentoService
    {
        Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoPedido);
    }
}

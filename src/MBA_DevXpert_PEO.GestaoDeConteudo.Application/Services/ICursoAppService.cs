using MBA_DevXpert_PEO.GestaoDeConteudo.Application.DTOs;

namespace MBA_DevXpert_PEO.GestaoDeConteudo.Application.Services
{
    public interface ICursoAppService
    {
        Task<IEnumerable<CursoDTO>> ObterTodos();
        Task<CursoDTO> ObterPorId(Guid id);
    }
}
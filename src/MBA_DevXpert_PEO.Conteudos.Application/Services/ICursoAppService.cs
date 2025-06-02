using MBA_DevXpert_PEO.Conteudos.Application.DTOs;

namespace MBA_DevXpert_PEO.Conteudos.Application.Services
{
    public interface ICursoAppService
    {
        Task<IEnumerable<CursoDTO>> ObterTodos();
        Task<CursoDTO> ObterPorId(Guid id);
    }
}
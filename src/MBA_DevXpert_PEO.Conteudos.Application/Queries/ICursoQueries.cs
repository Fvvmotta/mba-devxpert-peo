using System.Collections.Generic;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Conteudos.Application.DTOs;

namespace MBA_DevXpert_PEO.Conteudos.Application.Queries

{
    public interface ICursoQueries
    {
        Task<IEnumerable<CursoDTO>> ObterTodos();
        Task<CursoDTO?> ObterPorId(Guid id);
        Task<CursoDTO?> ObterPorCursoId(Guid cursoId);
        Task<CursoResumoDto?> ObterResumoCurso(Guid cursoId);
    }

}

using System.Collections.Generic;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Conteudos.Application.DTOs;

namespace MBA_DevXpert_PEO.Conteudos.Application.Queries

{
    public interface ICursoQueries
    {
        Task<IEnumerable<CursoDto>> ObterTodos();
        Task<CursoDto?> ObterPorId(Guid id);
        Task<CursoResumoDto?> ObterResumoCurso(Guid cursoId);
    }

}

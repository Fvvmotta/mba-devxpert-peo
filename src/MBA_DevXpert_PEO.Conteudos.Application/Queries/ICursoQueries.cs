using System.Collections.Generic;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Conteudos.Application.DTOs;

namespace MBA_DevXpert_PEO.Conteudos.Application.Queries

{
    public interface ICursoQueries
    {
        Task<CursoResumoDto> ObterResumoCurso(Guid cursoId);
        Task<IEnumerable<CursoDTO>> ObterTodos();
    }
}

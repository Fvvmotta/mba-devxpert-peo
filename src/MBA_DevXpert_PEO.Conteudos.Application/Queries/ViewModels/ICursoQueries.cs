using System.Collections.Generic;
using System.Threading.Tasks;

namespace Conteudos.Queries.ViewModels
{
    public interface ICursoQueries
    {
        Task<IEnumerable<CursoViewModel>> ObterTodos();
    }
}

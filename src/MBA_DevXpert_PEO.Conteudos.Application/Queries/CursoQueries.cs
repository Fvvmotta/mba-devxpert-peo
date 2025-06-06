using MBA_DevXpert_PEO.Conteudos.Application.DTOs;
using MBA_DevXpert_PEO.Conteudos.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace MBA_DevXpert_PEO.Conteudos.Application.Queries
{
    public class CursoQueries : ICursoQueries
    {
        private readonly GestaoConteudoContext _context;

        public CursoQueries(GestaoConteudoContext context)
        {
            _context = context;
        }

        public async Task<CursoResumoDto?> ObterResumoCurso(Guid cursoId)
        {
            return await _context.Cursos
                .Where(c => c.Id == cursoId)
                .Select(c => new CursoResumoDto
                {
                    CursoId = c.Id,
                    Nome = c.Nome,
                    Valor = c.Valor,
                    TotalAulas = c.Aulas.Count
                })
                .FirstOrDefaultAsync();
        }

        public Task<IEnumerable<CursoDTO>> ObterTodos()
        {
            throw new NotImplementedException();
        }
    }
}

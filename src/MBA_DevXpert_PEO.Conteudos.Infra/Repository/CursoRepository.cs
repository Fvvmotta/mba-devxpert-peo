using MBA_DevXpert_PEO.Core.Data;
using MBA_DevXpert_PEO.Conteudos.Domain.Entities;
using MBA_DevXpert_PEO.Conteudos.Domain.Repositories;
using MBA_DevXpert_PEO.Conteudos.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace MBA_DevXpert_PEO.Conteudos.Infra.Repository
{
    public class CursoRepository : ICursoRepository
    {
        private readonly GestaoConteudoContext _context;

        public CursoRepository(GestaoConteudoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Adicionar(Curso curso)
        {
            _context.Cursos.Add(curso);
        }

        public void Atualizar(Curso curso)
        {
            _context.Cursos.Update(curso);
        }

        public async Task<Curso?> ObterPorId(Guid id)
        {
            return await _context.Cursos
                .Include(c => c.Aulas)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Curso>> ObterTodos()
        {
            return await _context.Cursos
                .Include(c => c.Aulas)
                .AsNoTracking()
                .ToListAsync();
        }

        public void Remover(Curso curso)
        {
            _context.Cursos.Remove(curso);
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}


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

        public async Task<IEnumerable<CursoDTO>> ObterTodos()
        {
            var cursos = await _context.Cursos
                .Include(c => c.Aulas)
                .AsNoTracking()
                .ToListAsync();

            return cursos.Select(curso => new CursoDTO
            {
                Id = curso.Id,
                Nome = curso.Nome,
                CargaHoraria = curso.CargaHoraria,
                Autor = curso.Autor,
                Aulas = curso.Aulas.Select(aula => new AulaDTO
                {
                    Id = aula.Id,
                    Titulo = aula.Titulo,
                    Descricao = aula.Descricao,
                    MaterialUrl = aula.MaterialUrl
                }).ToList()
            });
        }

        public async Task<CursoDTO?> ObterPorId(Guid id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Aulas)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null) return null;

            return new CursoDTO
            {
                Id = curso.Id,
                Nome = curso.Nome,
                CargaHoraria = curso.CargaHoraria,
                Autor = curso.Autor,
                Aulas = curso.Aulas.Select(aula => new AulaDTO
                {
                    Id = aula.Id,
                    Titulo = aula.Titulo,
                    Descricao = aula.Descricao,
                    MaterialUrl = aula.MaterialUrl
                }).ToList()
            };
        }

        public async Task<CursoDTO?> ObterPorCursoId(Guid cursoId)
        {
            return await ObterPorId(cursoId);
        }

        public async Task<CursoResumoDto?> ObterResumoCurso(Guid cursoId)
        {
            return await _context.Cursos
                .AsNoTracking()
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
    }

}

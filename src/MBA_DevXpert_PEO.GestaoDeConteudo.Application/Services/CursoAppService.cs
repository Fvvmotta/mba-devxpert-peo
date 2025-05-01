using MBA_DevXpert_PEO.GestaoDeConteudo.Application.DTOs;
using MBA_DevXpert_PEO.GestaoDeConteudo.Domain.Repositories;

namespace MBA_DevXpert_PEO.GestaoDeConteudo.Application.Services
{
    public class CursoAppService : ICursoAppService
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoAppService(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<IEnumerable<CursoDTO>> ObterTodos()
        {
            var cursos = await _cursoRepository.ObterTodos();

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
            var curso = await _cursoRepository.ObterPorId(id);

            if (curso == null)
                return null;

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
    }
}

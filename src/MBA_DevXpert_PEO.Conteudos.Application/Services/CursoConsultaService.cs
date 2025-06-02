using MBA_DevXpert_PEO.Core.DomainObjects.DTO;
using MBA_DevXpert_PEO.Core.DomainObjects.Services;
using MBA_DevXpert_PEO.Conteudos.Domain.Repositories;

namespace MBA_DevXpert_PEO.Conteudos.Application.Services
{
    public class CursoConsultaService : ICursoConsultaService
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoConsultaService(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public async Task<IEnumerable<CursoDto>> ObterTodos()
        {
            var cursos = await _cursoRepository.ObterTodos();

            return cursos.Select(curso => new CursoDto
            {
                Id = curso.Id,
                Nome = curso.Nome,
                Autor = curso.Autor,
                CargaHoraria = curso.CargaHoraria
            });
        }
    }
}

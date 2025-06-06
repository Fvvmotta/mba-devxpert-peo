using System;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Core.Data;
using MBA_DevXpert_PEO.Conteudos.Domain.Entities;

namespace MBA_DevXpert_PEO.Conteudos.Domain.Repositories
{
    public interface ICursoRepository : IRepository<Curso>
    {
        Task<Curso> ObterPorId(Guid id);
        Task<IEnumerable<Curso>> ObterTodos();
        void Adicionar(Curso curso);
        void AdicionarAula(Aula aula);
        void Atualizar(Curso curso);
        void Remover(Curso curso);
    }

}
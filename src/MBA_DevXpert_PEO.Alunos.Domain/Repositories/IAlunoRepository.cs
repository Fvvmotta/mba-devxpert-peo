using System;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using MBA_DevXpert_PEO.Core.Data;

namespace MBA_DevXpert_PEO.Alunos.Domain.Repositories
{
    public interface IAlunoRepository : IRepository<Aluno>
    {
        Task<Aluno?> ObterPorId(Guid id);
        Task<IEnumerable<Aluno>> ObterTodos();
        Task<IEnumerable<Aluno>> ObterTodosComMatriculas(); 
        void Adicionar(Aluno aluno);
        void AdicionarMatricula(Matricula matricula);
        void Atualizar(Aluno aluno);
        void Remover(Aluno aluno);
    }
}

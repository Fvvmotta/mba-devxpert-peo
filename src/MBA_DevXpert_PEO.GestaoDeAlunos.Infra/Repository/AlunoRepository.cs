using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MBA_DevXpert_PEO.GestaoDeAlunos.Domain.Entities;
using MBA_DevXpert_PEO.GestaoDeAlunos.Domain.Repositories;
using MBA_DevXpert_PEO.GestaoDeAlunos.Infra.Context;
using MBA_DevXpert_PEO.Core.Data;

namespace MBA_DevXpert_PEO.GestaoDeAlunos.Infra.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly GestaoDeAlunosContext _context;

        public AlunoRepository(GestaoDeAlunosContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Aluno?> ObterPorId(Guid id)
        {
            return await _context.Alunos
                .Include(a => a.Matriculas)
                    .ThenInclude(m => m.Certificado)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Aluno>> ObterTodos()
        {
            return await _context.Alunos
                .Include(a => a.Matriculas)
                    .ThenInclude(m => m.Certificado)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<Aluno>> ObterTodosComMatriculas()
        {
            return await _context.Alunos
                .Include(a => a.Matriculas)
                    .ThenInclude(m => m.Certificado)
                .ToListAsync();
        }

        public void Adicionar(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
        }

        public void Atualizar(Aluno aluno)
        {
            _context.Alunos.Update(aluno);
        }
        public void Remover(Aluno aluno)
        {
            _context.Alunos.Remove(aluno);
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }

}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MBA_DevXpert_PEO.Conteudos.Infra.Context;
using MBA_DevXpert_PEO.Alunos.Infra.Context;
using MBA_DevXpert_PEO.Pagamentos.Infra.Context;

namespace MBA_DevXpert_PEO.Api.Controllers
{
    [ApiController]
    [Route("api/diagnostico")]
    public class DiagnosticoController : Controller
    {
        private readonly GestaoConteudoContext _conteudoContext;
        private readonly AlunosContext _alunosContext;
        private readonly PagamentosContext _pagamentosContext;

        public DiagnosticoController(
            GestaoConteudoContext conteudoContext,
            AlunosContext alunosContext,
            PagamentosContext pagamentosContext)
        {
            _conteudoContext = conteudoContext;
            _alunosContext = alunosContext;
            _pagamentosContext = pagamentosContext;
        }

        [HttpGet("teste-base")]
        public async Task<IActionResult> ObterBaseDeTeste()
        {
            var curso = await _conteudoContext.Cursos
                .Include(c => c.Aulas)
                .FirstOrDefaultAsync();

            var aluno = await _alunosContext.Alunos
                .Include(a => a.Matriculas)
                    .ThenInclude(m => m.Certificado)
                .FirstOrDefaultAsync();

            var pagamento = await _pagamentosContext.Pagamentos
                .FirstOrDefaultAsync();

            return Ok(new
            {
                Curso = new
                {
                    curso?.Id,
                    curso?.Nome,
                    curso?.Autor,
                    curso?.CargaHoraria,
                    Aulas = curso?.Aulas.Select(a => new { a.Titulo, a.Descricao, a.MaterialUrl })
                },
                Aluno = new
                {
                    aluno?.Id,
                    aluno?.Nome,
                    aluno?.Email,
                    Matriculas = aluno?.Matriculas.Select(m => new
                    {
                        m.Id,
                        m.CursoId,
                        m.DataMatricula,
                        m.Status,
                        Certificado = m.Certificado != null ? new
                        {
                            m.Certificado.Id,
                            m.Certificado.DataEmissao
                        } : null
                    })
                },
                Pagamento = pagamento != null ? new
                {
                    pagamento.Id,
                    pagamento.MatriculaId,
                    pagamento.Valor,
                    pagamento.DataPagamento,
                    Status = pagamento.Status?.Valor.ToString()
                } : null
            });
        }
    }
}

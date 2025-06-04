using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Alunos.Domain.Repositories;
using MBA_DevXpert_PEO.Alunos.Application.Commands;
using MBA_DevXpert_PEO.Alunos.Domain.Entities;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.Alunos.Application.Events;

namespace MBA_DevXpert_PEO.Alunos.Application.Handlers
{
    public class AlunoCommandHandler :
        IRequestHandler<CriarMatriculaCommand, bool>
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IMediatorHandler _mediator;

        public AlunoCommandHandler(IAlunoRepository alunoRepository, IMediatorHandler mediator)
        {
            _alunoRepository = alunoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(CriarMatriculaCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido())
            {
                await _mediator.PublicarNotificacao(new DomainNotification("Command inválido", "Dados da matrícula são inválidos."));
                return false;
            }

            var aluno = await _alunoRepository.ObterPorId(command.AlunoId);
            if (aluno == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("Aluno", "Aluno não encontrado."));
                return false;
            }

            var matricula = new Matricula(command.CursoId);
            aluno.Matricular(matricula);
            _alunoRepository.Atualizar(aluno);

            var sucesso = await _alunoRepository.UnitOfWork.Commit();

            if (sucesso)
            {
                await _mediator.PublicarEvento(new MatriculaCriadaEvent(aluno.Id, command.CursoId, matricula.Id));
            }

            return sucesso;
        }
        public async Task<bool> Handle(FinalizarCursoCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido())
            {
                await _mediator.PublicarNotificacao(new DomainNotification("Command inválido", "Dados inválidos para finalização do curso."));
                return false;
            }

            var aluno = await _alunoRepository.ObterPorId(command.AlunoId);
            if (aluno == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("Aluno", "Aluno não encontrado."));
                return false;
            }

            try
            {
                aluno.ConcluirMatricula(
                    command.MatriculaId,
                    command.AlunoNome,
                    command.CursoNome,
                    command.CargaHoraria,
                    DateTime.UtcNow
                );
            }
            catch (Exception ex)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("Erro ao concluir", ex.Message));
                return false;
            }

            _alunoRepository.Atualizar(aluno);
            var sucesso = await _alunoRepository.UnitOfWork.Commit();

            if (sucesso)
            {
                var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == command.MatriculaId);
                var certificadoId = matricula?.Certificado?.Id ?? Guid.Empty;
                await _mediator.PublicarEvento(new CursoFinalizadoEvent(command.AlunoId, command.MatriculaId, certificadoId));
            }
            return sucesso;
        }

        public async Task<bool> Handle(AtualizarAlunoCommand command, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorId(command.AlunoId);
            if (aluno == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("Aluno", "Aluno não encontrado."));
                return false;
            }

            aluno.AtualizarDados(command.Nome, command.Email);
            _alunoRepository.Atualizar(aluno);
            return await _alunoRepository.UnitOfWork.Commit();
        }


    }
}

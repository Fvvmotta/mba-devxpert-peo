using MediatR;
using MBA_DevXpert_PEO.Alunos.Domain.Repositories;
using MBA_DevXpert_PEO.Alunos.Application.Commands;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.Alunos.Application.Events;
using Alunos.Commands;
using MBA_DevXpert_PEO.Alunos.Domain.Entities;

namespace MBA_DevXpert_PEO.Alunos.Application.Handlers
{
    public class AlunoCommandHandler :
        IRequestHandler<CriarMatriculaCommand, bool>,
        IRequestHandler<FinalizarCursoCommand, bool>,
        IRequestHandler<FinalizarAulaCommand, bool>,
        IRequestHandler<CriarAlunoCommand, bool>,
        IRequestHandler<AtualizarAlunoCommand, bool>
        
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

            var matricula = new Matricula(command.AlunoId, command.CursoId, command.Valor);
            matricula.DefinirTotalAulas(command.TotalAulas);

            aluno.Matricular(matricula);
            _alunoRepository.AdicionarMatricula(matricula);

            var sucesso = await _alunoRepository.UnitOfWork.Commit();

            if (sucesso)
            {
                await _mediator.PublicarEvento(new MatriculaCriadaEvent(aluno.Id, command.CursoId, matricula.Id));
            }

            return sucesso;
        }

        public async Task<bool> Handle(FinalizarAulaCommand command, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorId(command.AlunoId);
            if (aluno == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("Aluno", "Aluno não encontrado."));
                return false;
            }

            var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == command.MatriculaId);
            if (matricula == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("Matrícula", "Matrícula não encontrada."));
                return false;
            }

            matricula.DefinirTotalAulas(command.TotalAulasCurso); 
            matricula.RegistrarAulaConcluida();

            _alunoRepository.Atualizar(aluno);
            var sucesso = await _alunoRepository.UnitOfWork.Commit();

            if (sucesso)
            {
                await _mediator.PublicarEvento(new AulaFinalizadaEvent(command.AlunoId, command.MatriculaId));
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

            var sucesso = aluno.ConcluirMatricula(
                command.MatriculaId,
                command.AlunoNome,
                command.CursoNome,
                command.CargaHoraria,
                DateTime.UtcNow,
                out var erro
            );

            if (!sucesso)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("Erro ao concluir matrícula", erro));
                return false;
            }

            _alunoRepository.Atualizar(aluno);
            await _alunoRepository.UnitOfWork.Commit();

            var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == command.MatriculaId);
            var certificadoId = matricula?.Certificado?.Id ?? Guid.Empty;

            await _mediator.PublicarEvento(new CursoFinalizadoEvent(command.AlunoId, command.MatriculaId, certificadoId));

            return true;
        }
        public async Task<bool> Handle(CriarAlunoCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido())
            {
                await _mediator.PublicarNotificacao(new DomainNotification("Command inválido", "Dados inválidos para criação do aluno."));
                return false;
            }

            var aluno = Aluno.CriarComId(command.AlunoId, command.Nome, command.Email);
            _alunoRepository.Adicionar(aluno);
            var sucesso = await _alunoRepository.UnitOfWork.Commit();
            if (!sucesso) return false;

            await _mediator.PublicarEvento(new AlunoCriadoEvent(aluno.Id, aluno.Nome, aluno.Email));

            return true;
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
            var sucesso = await _alunoRepository.UnitOfWork.Commit();
            if (sucesso)
            {
                await _mediator.PublicarEvento(new AlunoAtualizadoEvent(command.AlunoId));
            }
            return sucesso;

        }


    }
}

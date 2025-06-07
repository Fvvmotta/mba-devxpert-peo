using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MBA_DevXpert_PEO.Core.Communication.Mediator;
using MBA_DevXpert_PEO.Core.Messages.CommonMessages.Notifications;
using MBA_DevXpert_PEO.Conteudos.Domain.Repositories;
using MBA_DevXpert_PEO.Conteudos.Application.Commands;
using MBA_DevXpert_PEO.Core.Messages;
using MBA_DevXpert_PEO.Conteudos.Domain.Entities;
using MBA_DevXpert_PEO.Conteudos.Domain.ValueObjects;
using MBA_DevXpert_PEO.Conteudos.Application.Events;

namespace MBA_DevXpert_PEO.Conteudos.Application.Handlers
{
    public class CursoCommandHandler :
        IRequestHandler<CriarCursoCommand, Guid?>,
        IRequestHandler<AdicionarAulaCommand, bool>,
        IRequestHandler<AtualizarCursoCommand, bool>,
        IRequestHandler<ExcluirCursoCommand, bool>,
        IRequestHandler<ExcluirAulaCursoCommand, bool>
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public CursoCommandHandler(ICursoRepository cursoRepository, IMediatorHandler mediatorHandler)
        {
            _cursoRepository = cursoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<Guid?> Handle(CriarCursoCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido())
            {
                await NotificarErros(command);
                return null;
            }

            var conteudo = ConteudoProgramatico.Criar(command.DescricaoConteudoProgramatico);
            var curso = new Curso(command.Nome, command.Autor, command.CargaHoraria, conteudo);

            _cursoRepository.Adicionar(curso);
            var sucesso = await _cursoRepository.UnitOfWork.Commit();

            if (!sucesso) return null;

            await _mediatorHandler.PublicarEvento(new CursoCriadoEvent(curso.Id, curso.Nome, curso.Autor));

            return curso.Id;
        }

        public async Task<bool> Handle(AdicionarAulaCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido())
            {
                await NotificarErros(command);
                return false;
            }

            var curso = await _cursoRepository.ObterPorId(command.CursoId);
            if (curso == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("Curso", "Curso não encontrado."));
                return false;
            }

            var aula = curso.AdicionarAula(command.Titulo, command.Descricao, command.MaterialUrl);
            _cursoRepository.AdicionarAula(aula);

            var sucesso = await _cursoRepository.UnitOfWork.Commit();

            if (sucesso)
            {
                await _mediatorHandler.PublicarEvento(new AulaAdicionadaEvent(command.CursoId, aula.Id, aula.Titulo));
            }

            return sucesso;
        }


        public async Task<bool> Handle(AtualizarCursoCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido())
            {
                await NotificarErros(command);
                return false;
            }

            var curso = await _cursoRepository.ObterPorId(command.Id);

            if (curso == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("Curso", "Curso não encontrado."));
                return false;
            }

            var novoConteudo = ConteudoProgramatico.Criar(command.DescricaoConteudoProgramatico);
            curso.AtualizarCurso(command.Nome, command.CargaHoraria, command.Autor, novoConteudo);

            _cursoRepository.Atualizar(curso);
            var sucesso = await _cursoRepository.UnitOfWork.Commit();
            if (sucesso)
            {
                await _mediatorHandler.PublicarEvento(new CursoAtualizadoEvent(curso.Id, curso.Nome));
            }

            return sucesso;
        }

        public async Task<bool> Handle(ExcluirCursoCommand command, CancellationToken cancellationToken)
        {
            var curso = await _cursoRepository.ObterPorId(command.Id);

            if (curso == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("Curso", "Curso não encontrado."));
                return false;
            }

            _cursoRepository.Remover(curso);

            var sucesso = await _cursoRepository.UnitOfWork.Commit();

            if (sucesso)
            {
                await _mediatorHandler.PublicarEvento(new CursoRemovidoEvent(command.Id));
            }

            return sucesso;
        }

        public async Task<bool> Handle(ExcluirAulaCursoCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido())
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("Aula", "Dados inválidos para exclusão de aula."));
                return false;
            }

            var curso = await _cursoRepository.ObterPorId(command.CursoId);
            if (curso == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("Curso", "Curso não encontrado."));
                return false;
            }

            Console.WriteLine("Antes de remover: " + curso.Aulas.Count);
            curso.RemoverAula(command.AulaId);
            Console.WriteLine("Depois de remover: " + curso.Aulas.Count);
            _cursoRepository.Atualizar(curso);

            var sucesso = await _cursoRepository.UnitOfWork.Commit();

            if (sucesso)
            {
                await _mediatorHandler.PublicarEvento(new AulaRemovidaEvent(command.CursoId, command.AulaId));
            }

            return sucesso;
        }

        private async Task NotificarErros<TResponse>(Command<TResponse> command)
        {
            foreach (var error in command.ValidationResult.Errors)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(command.MessageType, error.ErrorMessage));
            }
        }
    }
}

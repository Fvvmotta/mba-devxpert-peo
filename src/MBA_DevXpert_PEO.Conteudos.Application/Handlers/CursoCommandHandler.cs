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

namespace MBA_DevXpert_PEO.Conteudos.Application.Handlers
{
    public class CursoCommandHandler :
        IRequestHandler<CriarCursoCommand, Guid?>,
        IRequestHandler<AdicionarAulaCommand, bool>,
        IRequestHandler<UpdateCursoCommand, bool>,
        IRequestHandler<DeleteCursoCommand, bool>
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

            return sucesso ? curso.Id : null;
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

            curso.AdicionarAula(command.Titulo, command.Descricao, command.MaterialUrl);
            _cursoRepository.Atualizar(curso);

            return await _cursoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(UpdateCursoCommand command, CancellationToken cancellationToken)
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
            return await _cursoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(DeleteCursoCommand command, CancellationToken cancellationToken)
        {
            var curso = await _cursoRepository.ObterPorId(command.Id);

            if (curso == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("Curso", "Curso não encontrado."));
                return false;
            }

            _cursoRepository.Remover(curso);

            return await _cursoRepository.UnitOfWork.Commit();
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

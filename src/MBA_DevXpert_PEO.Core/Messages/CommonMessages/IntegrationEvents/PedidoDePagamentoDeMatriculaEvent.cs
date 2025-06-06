using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBA_DevXpert_PEO.Core.Messages.CommonMessages.IntegrationEvents
{
    using MBA_DevXpert_PEO.Core.Messages;

    public class PedidoDePagamentoDeMatriculaEvent : Event
    {
        public Guid MatriculaId { get; }
        public Guid AlunoId { get; }
        public decimal Valor { get; }
        public string NomeCartao { get; }
        public string NumeroCartao { get; }
        public string ExpiracaoCartao { get; }
        public string CvvCartao { get; }

        public PedidoDePagamentoDeMatriculaEvent(Guid matriculaId, Guid alunoId, decimal valor,
            string nomeCartao, string numeroCartao, string expiracaoCartao, string cvvCartao)
        {
            MatriculaId = matriculaId;
            AlunoId = alunoId;
            Valor = valor;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CvvCartao = cvvCartao;
            AggregateId = matriculaId;
        }
    }

}

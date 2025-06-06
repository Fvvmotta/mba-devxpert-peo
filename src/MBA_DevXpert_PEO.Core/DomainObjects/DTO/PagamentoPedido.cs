using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBA_DevXpert_PEO.Core.DomainObjects.DTO
{
    public class PagamentoPedido
    {
        public Guid MatriculaId { get; set; }
        public Guid AlunoId { get; set; }
        public decimal Valor { get; set; }
        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string ExpiracaoCartao { get; set; }
        public string CvvCartao { get; set; }
    }
}


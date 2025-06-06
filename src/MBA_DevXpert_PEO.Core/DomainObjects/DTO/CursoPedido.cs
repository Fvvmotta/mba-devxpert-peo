using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBA_DevXpert_PEO.Core.DomainObjects.DTO
{
    public class CursoPedido
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Autor { get; set; }
        public int CargaHoraria { get; set; }
        public decimal Valor { get; set; }
    }
}

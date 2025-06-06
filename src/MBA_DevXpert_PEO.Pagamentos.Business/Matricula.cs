using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBA_DevXpert_PEO.Pagamentos.Business
{
    public class Matricula
    {
        public Guid Id { get; set; }
        public decimal Valor { get; set; }
        public List<Curso> Cursos { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.Conteudos.Application.Commands
{
    public class AdicionarAulaCommand : Command<bool>
    {
        public Guid CursoId { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public string MaterialUrl { get; private set; }

        public AdicionarAulaCommand(Guid cursoId, string titulo, string descricao, string materialUrl = null)
        {
            CursoId = cursoId;
            Titulo = titulo;
            Descricao = descricao;
            MaterialUrl = materialUrl;
        }

        public bool EhValido()
        {
            return
                CursoId != Guid.Empty &&
                !string.IsNullOrWhiteSpace(Titulo) &&
                !string.IsNullOrWhiteSpace(Descricao);
        }
    }

}

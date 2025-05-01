using MBA_DevXpert_PEO.Core.Messages;

namespace MBA_DevXpert_PEO.GestaoDeConteudo.Application.Commands
{
    public class UpdateCursoCommand : Command<bool>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int CargaHoraria { get; set; }
        public string Autor { get; set; }
        public string DescricaoConteudoProgramatico { get; set; }


        public UpdateCursoCommand(Guid id, string nome, string autor, int cargaHoraria, string descricaoConteudoProgramatico)
        {
            AggregateId = id;
            Id = id;
            Nome = nome;
            Autor = autor;
            CargaHoraria = cargaHoraria;
            DescricaoConteudoProgramatico = descricaoConteudoProgramatico;
        }

        public override bool EhValido()
        {
            return true;
        }
    }
}

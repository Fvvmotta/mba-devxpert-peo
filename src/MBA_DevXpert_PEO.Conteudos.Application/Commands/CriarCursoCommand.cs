using MBA_DevXpert_PEO.Core.Messages;
namespace MBA_DevXpert_PEO.Conteudos.Application.Commands
{
    public class CriarCursoCommand : Command<Guid?>
    {
        public string Nome { get; }
        public string Autor { get; }
        public int CargaHoraria { get; }
        public string DescricaoConteudoProgramatico { get; }

        public CriarCursoCommand(string nome, string autor, int cargaHoraria, string descricaoConteudoProgramatico)
        {
            Nome = nome;
            Autor = autor;
            CargaHoraria = cargaHoraria;
            DescricaoConteudoProgramatico = descricaoConteudoProgramatico;
        }

        public override bool EhValido()
        {
            // Adicione sua validação aqui
            return true;
        }
    }
}

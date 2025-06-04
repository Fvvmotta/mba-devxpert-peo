using MBA_DevXpert_PEO.Core.DomainObjects;

namespace MBA_DevXpert_PEO.Alunos.Domain.ValueObjects
{
    public class HistoricoAprendizado : ValueObject
    {
        public int TotalAulas { get; }
        public int AulasConcluidas { get; }

        public bool TodasAulasConcluidas => AulasConcluidas >= TotalAulas && TotalAulas > 0;

        protected HistoricoAprendizado() { }

        public HistoricoAprendizado(int totalAulas, int aulasConcluidas)
        {
            Validacoes.ValidarMinimoMaximo(totalAulas, 0, 1000, "Total de aulas inválido");
            Validacoes.ValidarMinimoMaximo(aulasConcluidas, 0, totalAulas, "Quantidade de aulas concluídas inválida");

            TotalAulas = totalAulas;
            AulasConcluidas = aulasConcluidas;
        }

        public HistoricoAprendizado DefinirTotalAulas(int novoTotal)
        {
            return new HistoricoAprendizado(novoTotal, AulasConcluidas);
        }

        public HistoricoAprendizado RegistrarAulaConcluida()
        {
            var novasConcluidas = AulasConcluidas + 1;
            return new HistoricoAprendizado(TotalAulas, novasConcluidas > TotalAulas ? TotalAulas : novasConcluidas);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TotalAulas;
            yield return AulasConcluidas;
        }
    }
}

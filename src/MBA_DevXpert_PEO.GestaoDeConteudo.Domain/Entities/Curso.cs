using MBA_DevXpert_PEO.Core.DomainObjects;
using MBA_DevXpert_PEO.GestaoDeConteudo.Domain.Entities;
using MBA_DevXpert_PEO.GestaoDeConteudo.Domain.ValueObjects;

public class Curso : Entity, IAggregateRoot
{
    public string Nome { get; private set; }
    public string Autor { get; private set; }
    public int CargaHoraria { get; private set; }
    public List<Aula> Aulas { get; private set; }
    public ConteudoProgramatico ConteudoProgramatico { get; private set; }

    protected Curso() { }

    public Curso(string nome, string autor, int cargaHoraria, ConteudoProgramatico conteudoProgramatico)
    {
        SetNome(nome);
        SetAutor(autor);
        SetCargaHoraria(cargaHoraria);
        DefinirConteudoProgramatico(conteudoProgramatico);
        Aulas = new List<Aula>();
    }

    public void SetNome(string nome)
    {
        Validacoes.ValidarSeVazio(nome, "Nome do curso não pode ser vazio.");
        Nome = nome;
    }

    public void SetAutor(string autor)
    {
        Validacoes.ValidarSeVazio(autor, "Nome do autor não pode ser vazio.");
        Autor = autor;
    }

    public void SetCargaHoraria(int cargaHoraria)
    {
        Validacoes.ValidarSeMenorQue(cargaHoraria, 1, "Carga horária deve ser maior que zero.");
        CargaHoraria = cargaHoraria;
    }

    public void AdicionarAula(string titulo, string descricao, string materialUrl = null)
    {
        var aula = new Aula(titulo, descricao, materialUrl);
        Aulas.Add(aula);
    }

    public void AtualizarCurso(string nome, int cargaHoraria, string autor, ConteudoProgramatico novoConteudo)
    {
        SetNome(nome);
        SetAutor(autor);
        SetCargaHoraria(cargaHoraria);
        DefinirConteudoProgramatico(novoConteudo);
    }

    private void DefinirConteudoProgramatico(ConteudoProgramatico conteudo)
    {
        Validacoes.ValidarSeNulo(conteudo, "Conteúdo programático inválido.");
        ConteudoProgramatico = conteudo;
    }
}

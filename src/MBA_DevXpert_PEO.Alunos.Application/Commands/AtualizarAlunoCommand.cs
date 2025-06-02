using MBA_DevXpert_PEO.Core.Messages;

public class AtualizarAlunoCommand : Command<bool>
{
    public Guid AlunoId { get; init; }
    public string Nome { get; init; }
    public string Email { get; init; }

    public AtualizarAlunoCommand(Guid alunoId, string nome, string email)
    {
        AlunoId = alunoId;
        Nome = nome;
        Email = email;
    }

    public override bool EhValido()
    {
        return AlunoId != Guid.Empty && !string.IsNullOrWhiteSpace(Nome) && !string.IsNullOrWhiteSpace(Email);
    }
}

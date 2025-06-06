public class AtualizarAlunoIdentityDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string? NovaSenha { get; set; } 
    public string? ConfirmarSenha { get; set; } 
}
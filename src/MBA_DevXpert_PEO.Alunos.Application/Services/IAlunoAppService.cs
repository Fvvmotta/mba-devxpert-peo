using MBA_DevXpert_PEO.Alunos.Application.DTOs;

public interface IAlunoAppService
{
    Task<IEnumerable<AlunoComMatriculasDto>> ObterAlunosComMatriculas();
}
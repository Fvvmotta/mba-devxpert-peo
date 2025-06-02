using MBA_DevXpert_PEO.Core.DomainObjects.DTO;

public interface IAlunoAppService
{
    Task<IEnumerable<MatriculaDetalhadaDto>> ObterMatriculasDetalhadas();
}
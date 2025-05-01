using System.Threading.Tasks;

namespace MBA_DevXpert_PEO.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
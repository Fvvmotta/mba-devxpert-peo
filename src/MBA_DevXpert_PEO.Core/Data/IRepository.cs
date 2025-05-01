using System;
using MBA_DevXpert_PEO.Core.DomainObjects;

namespace MBA_DevXpert_PEO.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
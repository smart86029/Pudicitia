using System.Threading.Tasks;

namespace Pudicitia.Common.Domain
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}
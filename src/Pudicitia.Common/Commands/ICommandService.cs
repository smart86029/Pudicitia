using System.Threading.Tasks;

namespace Pudicitia.Common.Commands
{
    public interface ICommandService
    {
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command);
    }
}
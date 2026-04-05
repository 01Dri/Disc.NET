using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands
{
    public interface ICommand<T> where T : ContextBase
    {
    }
}

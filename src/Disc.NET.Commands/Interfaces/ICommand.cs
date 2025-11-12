using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands.Interfaces
{
    public interface ICommand<T> where T : IContext
    {
        Task<bool> RunAsync(T context);
    }
}
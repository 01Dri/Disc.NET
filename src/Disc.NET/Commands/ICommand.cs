using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disc.NET.Models;
using Disc.NET.Models.Commands;

namespace Disc.NET.Commands
{
    public interface ICommand<T> where T : IContext
    {
        Task<bool> RunAsync(T context);
    }
}

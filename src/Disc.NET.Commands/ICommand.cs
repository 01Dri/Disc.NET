using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands
{
    public interface ICommand<T> where T : IContext
    {
    }
}

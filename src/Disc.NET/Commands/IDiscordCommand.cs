using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disc.NET.Commands
{
    public interface IDiscordCommand
    {
        Task<bool> RunAsync();
    }
}

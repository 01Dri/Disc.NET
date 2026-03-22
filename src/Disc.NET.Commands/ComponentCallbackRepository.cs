using Disc.NET.Commands.Contexts;
using System.Collections.Concurrent;

namespace Disc.NET.Commands
{
    internal static class ComponentCallbackRepository
    {
        private static readonly ConcurrentDictionary<string, Func<ContextBase, Task>> _callbackAsync = new();

        public static void RegisterCallback(string customId, Func<ContextBase, Task> callback)
        {
            _callbackAsync[customId] = callback;
        }

        public static void UnregisterCallback(string customId)
        {
            _callbackAsync.TryRemove(customId, out _);
        }

        public static async Task<bool> InvokeCallbackAsync(string customId, ContextBase context)
        {
            if (_callbackAsync.TryGetValue(customId, out var callback))
            {
                await callback(context);
                return true;
            }

            return false;
        }
    }
}
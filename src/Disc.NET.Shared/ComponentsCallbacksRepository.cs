namespace Disc.NET.Commands
{
    public class ComponentsCallbacksRepository
    {

        private static ComponentsCallbacksRepository? _instance;

        private readonly Dictionary<string, Func<bool>> _callbacks;
        public static ComponentsCallbacksRepository Instance => _instance ??= new ComponentsCallbacksRepository();

        public ComponentsCallbacksRepository()
        {
            _callbacks = new Dictionary<string, Func<bool>>();
        }
        public void SaveCallback(string callbackId, Func<bool> callback)
        {
            _callbacks.Add(callbackId, callback);
        }

        public Func<bool>? GetCallback(string callbackId)
        {
            if (_callbacks.TryGetValue(callbackId, out var callback))
            {
                return callback;
            }
            return null;
        }
    }
}


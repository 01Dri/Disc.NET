using Autofac;
using Disc.NET.Commands;
using System.Reflection;

namespace Disc.NET.Configuration
{
    public class DiscNetContainer
    {
        private static ContainerBuilder _containerBuilder;
        private IContainer? _container;
        private static DiscNetContainer? _instance;

        public static DiscNetContainer GetInstance()
        {
            if (_instance == null)
            {
                _containerBuilder = new ContainerBuilder();
                _instance = new DiscNetContainer();
            }

            return _instance;
        }

        public DiscNetContainer RegisterDependency<TInterface, TKInstance>() where TInterface : notnull where TKInstance : notnull
        {
            _containerBuilder.RegisterType<TKInstance>().As<TInterface>();
            return this;
        }

        public DiscNetContainer RegisterDependency<TKInstance>()
            where TKInstance : notnull
        {
            _containerBuilder.RegisterType<TKInstance>();
            return this;
        }


        public void RegisterDependencies()
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
                throw new InvalidOperationException("Could not determine the entry assembly.");

            var commandType = assembly
                .GetTypes()
                .Where(t => typeof(ISlashCommand).IsAssignableFrom(t) || typeof(IPrefixCommand).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract)
                .Where(t => !t.IsInterface).ToList();

            commandType.ForEach(x => _containerBuilder!.RegisterType(x));
        }

        public DiscNetContainer WithHttpClient()
        {
            _containerBuilder!.Register(c => new HttpClient())
                .As<HttpClient>();
            return this;
        }


        public IContainer Build()
        {
            if (_container != null)
            {
                return _container;
            }
            _container = _containerBuilder.Build();
            return _container;
        }
    }
}

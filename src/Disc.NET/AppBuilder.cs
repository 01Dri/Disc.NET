using Disc.NET.Configuration;

namespace Disc.NET
{
    public class AppBuilder
    {
        public DiscNetContainer Services { get; set; }
        public AppConfiguration? Configuration { get; set; }

        public AppBuilder()
        {
            Services = DiscNetContainer.GetInstance();
        }

        public AppBuilder AddConfiguration(AppConfiguration configuration)
        {
            Configuration = configuration;
            return this;
        } 

        public App Build()
        {
            Services.RegisterDependencies();
            if (Configuration == null)
            {
                var token = Environment.GetEnvironmentVariable("GENERIC_BOT_TOKEN");
                var applicationId = Environment.GetEnvironmentVariable("GENERIC_BOT_APPLICATION_ID");
                return new App(new AppConfiguration(token)
                {
                    ApplicationId = long.Parse(applicationId)
                });
            }
            return new App(Configuration!);
        }
    }
}

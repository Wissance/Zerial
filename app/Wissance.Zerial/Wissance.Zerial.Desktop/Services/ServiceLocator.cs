using Microsoft.Extensions.DependencyInjection;

namespace Wissance.Zerial.Desktop.Services
{
    public static class ServiceLocator
    {
        public static ServiceProvider Locate()
        {
            return Program.Services.BuildServiceProvider();
        }
    }
}
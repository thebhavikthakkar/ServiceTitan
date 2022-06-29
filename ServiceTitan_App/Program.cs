using ServiceTitan_App.Services;

namespace ServiceTitan_App
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            _ = log4net.Config.XmlConfigurator.Configure();
            ServiceTitanServices serviceTitanDBServices = new ServiceTitanServices();
            serviceTitanDBServices.ProcessWithList();
        }
    }
}

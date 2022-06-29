using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
namespace ServiceTitan.Services
{
    public class ExternalAPIServices
    {
        private readonly WebClient webClient;
        public ExternalAPIServices()
        {
            webClient = new WebClient();
        }
    }
}

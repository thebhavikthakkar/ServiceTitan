using ServiceTitan.Services;
using System;

namespace ServiceTitan
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceTitanDBServices serviceTitanDBServices = new ServiceTitanDBServices();
            serviceTitanDBServices.processWithList();
        }


    }
}

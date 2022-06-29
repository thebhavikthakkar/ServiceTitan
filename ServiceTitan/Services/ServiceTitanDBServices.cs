using ServiceTitan.Workers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceTitan.Services
{
    public class ServiceTitanDBServices
    {
        private readonly ClassServiceTitan serviceTitan;
        public ServiceTitanDBServices()
        {
            this.serviceTitan = new ClassServiceTitan();
        }

        public void processWithList()
        {
            var data = this.serviceTitan.GetClientMasters();
            if (data.Count > 0)
            {

            }
        }
    }
}

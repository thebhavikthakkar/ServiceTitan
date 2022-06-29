using System;
using System.Collections.Generic;
using System.Text;
using ServiceTitan.Data;

namespace ServiceTitan.Workers
{
    public class ClassServiceTitan
    {
        private readonly DBHelper dBHelper;
        public ClassServiceTitan()
        {
            dBHelper = new DBHelper();
        }

        public List<ClientMaster> GetClientMasters()
        {
            return dBHelper.GetClientMasters();
        }
    }
}

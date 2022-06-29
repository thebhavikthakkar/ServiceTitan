using System.Collections.Generic;
using System.Linq;

namespace ServiceTitan.Data.DBObjects
{
    public class ClientMasterHelper : DataContext
    {
        public List<ClientMaster> GetClientMasters()
        {
            return dbContext.ClientMasters.ToList();
        }
    }
}

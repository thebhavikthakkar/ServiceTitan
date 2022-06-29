using ServiceTitan.Data.DBObjects;
using System.Collections.Generic;
using System.Linq;

namespace ServiceTitan.Data
{
    public class DBHelper
    {
        private readonly ClientMasterHelper  clientMasterHelper;

        public DBHelper()
        {
            clientMasterHelper = new ClientMasterHelper();
        }
        public List<ClientMaster> GetClientMasters()
        {
            return clientMasterHelper.GetClientMasters();
        }
        public int InsertToSyncHistory(SyncHistory syncHistory)
        {
            dbContext.SyncHistories.Add(syncHistory);
            dbContext.SaveChanges();
            return syncHistory.id;
        }
    }
}

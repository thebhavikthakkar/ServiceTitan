using System.Linq;

namespace ServiceTitan.Data.DBObjects
{
    public class SyncHistoryHelper : DataContext
    {
        public int Insert(SyncHistory syncHistory)
        {
            _ = dbContext.SyncHistories.Add(syncHistory);
            _ = dbContext.SaveChanges();
            return syncHistory.id;
        }

        public void UpdateSyncEndOn(int id)
        {
            SyncHistory data = GetById(id);
            if (data != null)
            {
                data.syncEndOn = System.DateTime.UtcNow;
                _ = dbContext.SaveChanges();
            }
        }
        public SyncHistory GetById(int id)
        {
            return dbContext.SyncHistories.FirstOrDefault(x => x.id == id);
        }
    }
}

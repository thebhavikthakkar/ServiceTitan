using System;
using System.Linq;

namespace ServiceTitan.Data.DBObjects
{
    public class CapManagementHelper : DataContext
    {
        public CapManagementHelper()
        {
        }
        public void Insert(CapManagement capManagement)
        {
            _ = dbContext.CapManagements.Add(capManagement);
            _ = dbContext.SaveChanges();
        }
        public void Update(CapManagement capManagement)
        {
            CapManagement curObj = GetByClientIdAndDate(capManagement.date, capManagement.ClientId);
            if (curObj != null)
            {
                curObj.PendingCap = capManagement.PendingCap;
                _ = dbContext.SaveChanges();
            }
        }
        public CapManagement GetByClientIdAndDate(DateTime? dt, int? clientId)
        {
            return dbContext.CapManagements.FirstOrDefault(x => x.date == dt && x.ClientId == clientId);
        }
    }
}

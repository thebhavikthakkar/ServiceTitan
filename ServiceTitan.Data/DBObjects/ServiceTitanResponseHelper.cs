using System.Linq;

namespace ServiceTitan.Data.DBObjects
{
    public class ServiceTitanResponseHelper : DataContext
    {
        public int Insert(ServiceTitanResponse syncHistory)
        {
            _ = dbContext.ServiceTitanResponses.Add(syncHistory);
            _ = dbContext.SaveChanges();
            return syncHistory.Id;
        }

        public void Update(int LeadCallid, ServiceTitanResponse syncHistory)
        {
            ServiceTitanResponse curObj = GetByLeadCallId(LeadCallid);
            if (curObj != null)
            {
                curObj.FilePath = syncHistory.FilePath;
                curObj.assemblyairesponse = syncHistory.assemblyairesponse;
                curObj.ResponseStatus = syncHistory.ResponseStatus;
                curObj.ErrorCode = syncHistory.ErrorCode;
            }
            _ = dbContext.SaveChanges();
        }
        public ServiceTitanResponse GetByLeadCallId(int LeadCallid)
        {
            return dbContext.ServiceTitanResponses.FirstOrDefault(x => x.LeadCallid == LeadCallid);
        }

        public ServiceTitanResponse GetById(int id)
        {
            return dbContext.ServiceTitanResponses.FirstOrDefault(x => x.Id == id);
        }
    }
}

using ServiceTitan.Core;
using ServiceTitan.Data;
using System;

namespace ServiceTitan_App.Services
{
    public class ServiceTitanStatusHistoryServices : BaseServices
    {
        public void ManageStatus(StatusEnum statusEnum)
        {
            ServiceStatusHistory model = new ServiceStatusHistory()
            {
                ClientId = ClientConstants.ClientID,
                ServiceTitanResponseId = ClientConstants.ServiceTitanResponseId,
                SyncHistoryId = ClientConstants.syncHistoryId,
                Status = (int?)statusEnum,
                CreatedOn = DateTime.UtcNow
            };

            serviceTitanStatusHistory.Insert(model);
            Console.WriteLine("Manage Status: " + Newtonsoft.Json.JsonConvert.SerializeObject(model));
            log.Info("Manage Status: " + Newtonsoft.Json.JsonConvert.SerializeObject(model));

            ServiceTitanResponse responseData = serviceTitanResponseHelper.GetById(ClientConstants.ServiceTitanResponseId);
            if (responseData != null)
            {
                responseData.ResponseStatus = (int?)statusEnum;
                serviceTitanResponseHelper.Update((int)responseData.LeadCallid, responseData);
            }
        }
    }
}

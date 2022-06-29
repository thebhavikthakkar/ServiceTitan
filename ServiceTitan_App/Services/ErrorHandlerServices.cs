using ServiceTitan.Core;
using ServiceTitan.Data;
using System;

namespace ServiceTitan_App.Services
{
    public class ErrorHandlerServices : BaseServices
    {
        public void HandleError(Exception ex)
        {
            ErrorHandler model = new ErrorHandler()
            {
                ClientId = ClientConstants.ClientID,
                syncId = ClientConstants.syncHistoryId,
                createdOn = DateTime.UtcNow,
                ErrorMessage = ex.Message,
                ServiceTitanResponseId = ClientConstants.ServiceTitanResponseId,
                ErrorStacktrace = ex.StackTrace
            };
            errorHandlerHelper.Insert(model);

            Console.WriteLine("Error : " + Newtonsoft.Json.JsonConvert.SerializeObject(model));
            log.Error(model);
            log.Error(ex);
            ServiceTitanResponse responseData = serviceTitanResponseHelper.GetById(ClientConstants.ServiceTitanResponseId);
            if (responseData != null)
            {
                responseData.ErrorCode = ex.Message;
                serviceTitanResponseHelper.Update((int)responseData.LeadCallid, responseData);
                log.Info(responseData);
            }

        }
    }
}

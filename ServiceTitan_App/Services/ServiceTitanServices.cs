using Newtonsoft.Json.Linq;
using ServiceTitan.Core;
using ServiceTitan.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace ServiceTitan_App.Services
{
    public class ServiceTitanServices : BaseServices
    {
        private readonly ExternalAPIServices externalAPIServices;
        private readonly ServiceTitanStatusHistoryServices serviceTitanStatusHistoryServices;
        private readonly FileAndDirectoryService fileAndDirectoryService;
        private readonly ErrorHandlerServices errorHandlerServices;
        private readonly CapManagementService capManagementService;
        public ServiceTitanServices()
        {
            externalAPIServices = new ExternalAPIServices();
            serviceTitanStatusHistoryServices = new ServiceTitanStatusHistoryServices();
            fileAndDirectoryService = new FileAndDirectoryService();
            errorHandlerServices = new ErrorHandlerServices();
            capManagementService = new CapManagementService();
        }
        public void ProcessWithList()
        {
            try
            {
                log.Info("ProcessWithList Called");
                InsertToSyncHistory();
                List<ClientMaster> data = clientMaster.GetClientMasters();
                if (data.Count > 0)
                {
                    foreach (ClientMaster curObj in data)
                    {
                        log.Info(curObj);
                        ClientConstants.ClientID = curObj.Id;
                        ClientConstants.Tenant = (int)curObj.tenant;
                        ClientConstants.STClientID = curObj.STClientID;
                        ClientConstants.STClientSecret = curObj.STClientSecret;
                        ClientConstants.ServiceTitanJWTToken = string.Empty;
                        ClientConstants.TotalMonthlyCap = (int)curObj.Cap;
                        externalAPIServices.GetServiceTitanJWTAuthentication();
                        if (!string.IsNullOrWhiteSpace(ClientConstants.ServiceTitanJWTToken))
                        {
                            serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.JWTTokenGenerated);
                            fileAndDirectoryService.CreateDirectory();
                            if (string.IsNullOrWhiteSpace(curObj.MappedAgents))
                            {
                                FetchTheCallDetails();
                            }
                            else
                            {
                                string[] agentData = curObj.MappedAgents.Split(',');
                                foreach (string agent in agentData)
                                {
                                    if (string.IsNullOrWhiteSpace(agent))
                                    {
                                        FetchTheCallDetails(agent);
                                    }
                                }
                            }
                        }
                        else
                        {
                            serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.IssueWhileJWTToken);
                        }
                    }
                }
                UpdateSyncEndOn();
                log.Info("ProcessWithList End");
            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
                serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.IssueWhileJWTToken);
            }
        }

        public void UpdateSyncEndOn()
        {
            syncHistory.UpdateSyncEndOn(ClientConstants.syncHistoryId);
        }
        public void InsertToSyncHistory()
        {
            try
            {
                SyncHistory ObjSyncHistory = new SyncHistory()
                {
                    createdOn = DateTime.UtcNow,
                    syncStartOn = DateTime.UtcNow,
                    syncEndOn = DateTime.UtcNow
                };
                ClientConstants.syncHistoryId = syncHistory.Insert(ObjSyncHistory);
                log.Info(ObjSyncHistory);
            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
            }
        }

        private Dictionary<string, object> FetchTheDataFromAPI(int pageNo, string agentName, int retryAttempts = 0)
        {
            log.Info("fetchTheDataFromAPI called: Pageno: " + pageNo);
            Dictionary<string, object> processData = null;
            try
            {
                System.Net.Http.HttpResponseMessage data = externalAPIServices.GetCalls(pageNo, ClientConstants.Tenant, ClientConstants.ServiceTitanJWTToken, agentName);

                if (data != null)
                {
                    if (data.IsSuccessStatusCode)
                    {
                        string strData = data.Content.ReadAsStringAsync().Result;
                        log.Info(strData);
                        processData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(strData);
                        serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.CallApiSuccess);
                    }
                    else
                    {
                        if (data.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryAttempts <= 5)
                        {
                            retryAttempts++;
                            externalAPIServices.GetServiceTitanJWTAuthentication();
                            processData = FetchTheDataFromAPI(pageNo, agentName, retryAttempts);
                        }
                        else
                        {
                            serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.IssueWhileCallingCallsAPi);
                        }
                    }
                }
                log.Info("fetchTheDataFromAPI End: Pageno: " + pageNo);
            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
                serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.IssueWhileCallingCallsAPi);
            }
            return processData;
        }


        private void ProcessFileAsync(int LeadCallId, Stream ms)
        {
            try
            {
                log.Info("processFileAsync called");
                log.Info("LeadCallId : " + LeadCallId);

                fileAndDirectoryService.SaveTheFileAsync(LeadCallId, ms);
                string serverPath = fileAndDirectoryService.SaveFileToServerDirectory(LeadCallId);
                string transcriptFile = externalAPIServices.GetTranscriptFromAssemblyAI(serverPath);
                log.Info(serverPath);
                log.Info(transcriptFile);

                ServiceTitanResponse curObj = serviceTitanResponseHelper.GetByLeadCallId(LeadCallId);
                curObj.assemblyairesponse = transcriptFile;
                curObj.FilePath = serverPath;
                curObj.ResponseStatus = (int?)StatusEnum.Success;
                serviceTitanResponseHelper.Update(LeadCallId, curObj);

                log.Info(curObj);
                log.Info("processFileAsync End");
            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
                serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.IssueWithDownloadFile);
            }
        }


        private void FetchTheRecording(int LeadCallId, int retryAttempt = 0)
        {
            try
            {
                log.Info("fetchTheRecording called : LeadCallId : " + LeadCallId);
                System.Net.Http.HttpResponseMessage data = externalAPIServices.GetRecording(ClientConstants.Tenant, LeadCallId, ClientConstants.ServiceTitanJWTToken);

                if (data != null)
                {
                    if (data.IsSuccessStatusCode)
                    {
                        Stream strData = data.Content.ReadAsStreamAsync().Result;
                        log.Info(strData);
                        serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.DownloadFileSuccess);
                        ProcessFileAsync(LeadCallId, strData);
                    }
                    else
                    {
                        if (data.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryAttempt <= 5)
                        {
                            retryAttempt++;
                            externalAPIServices.GetServiceTitanJWTAuthentication();
                            FetchTheRecording(LeadCallId, retryAttempt);
                        }
                        else
                        {
                            serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.IssueWithDownloadFile);
                        }
                    }
                }
                log.Info("fetchTheRecording End : LeadCallId : " + LeadCallId);
            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
                serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.IssueWithDownloadFile);
            }
        }


        private void ProcessWithEachRow(Dictionary<string, object> keyValuePairs)
        {
            try
            {
                log.Info("ProcessWithEachRow called");
                log.Info(keyValuePairs);
                int leadCallId = 0;
                ClientConstants.ServiceTitanResponseId = 0;
                string from = null, to = null;
                TimeSpan duration = TimeSpan.Zero;
                DateTime? dt = null;
                if (keyValuePairs.ContainsKey("leadCall"))
                {
                    if (keyValuePairs["leadCall"] is JObject jobject)
                    {
                        Dictionary<string, object> leadCallResponse = jobject.ToObject<Dictionary<string, object>>();

                        if (!(leadCallResponse["id"] is null))
                        {
                            leadCallId = Convert.ToInt32(leadCallResponse["id"]);
                        }
                        if (!(leadCallResponse["duration"] is null))
                        {
                            duration = TimeSpan.Parse(Convert.ToString(leadCallResponse["duration"]));
                        }
                        if (!(leadCallResponse["from"] is null))
                        {
                            from = Convert.ToString(leadCallResponse["from"]);
                        }
                        if (!(leadCallResponse["to"] is null))
                        {
                            to = Convert.ToString(leadCallResponse["to"]);
                        }
                        if (!(leadCallResponse["receivedOn"] is null))
                        {
                            dt = Convert.ToDateTime(leadCallResponse["receivedOn"]);
                        }
                        if (leadCallId > 0)
                        {
                            ServiceTitanResponse checkForRecordExist = serviceTitanResponseHelper.GetByLeadCallId(leadCallId);
                            if (checkForRecordExist != null
                                && (checkForRecordExist.ResponseStatus == (int?)StatusEnum.Success
                                || checkForRecordExist.ResponseStatus == (int?)StatusEnum.NotHaveEnoughCap))
                            {
                                return;
                            }
                            ServiceTitanResponse serviceTitanResponse = new ServiceTitanResponse()
                            {
                                ClientId = ClientConstants.ClientID,
                                LeadCallid = leadCallId,
                                duration = duration,
                                from = from,
                                to = to,
                                RowServiceTitanJson = Newtonsoft.Json.JsonConvert.SerializeObject(keyValuePairs),
                                syncId = ClientConstants.syncHistoryId,
                                createdOn = DateTime.UtcNow,
                                ReceivedOn = dt
                            };
                            ClientConstants.ServiceTitanResponseId = checkForRecordExist != null ? checkForRecordExist.Id : serviceTitanResponseHelper.Insert(serviceTitanResponse);
                            if (checkForRecordExist == null && ClientConstants.TotalMonthlyCap > 0)
                            {
                                StatusEnum returnstatus = capManagementService.GetCapStatus(duration, dt);
                                serviceTitanStatusHistoryServices.ManageStatus(returnstatus);
                                if (returnstatus == StatusEnum.NotHaveEnoughCap)
                                {
                                    return;
                                }
                            }
                            FetchTheRecording(leadCallId);
                        }
                    }
                }
                log.Info("ProcessWithEachRow completed");
            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
            }
        }
        private Dictionary<string, object> ProcessWithResponse(int pageNo, string agentName)
        {
            Dictionary<string, object> keyValuePairs = null;
            try
            {
                keyValuePairs = FetchTheDataFromAPI(pageNo, agentName);
                List<Dictionary<string, object>> processData = (keyValuePairs["data"] as JArray).ToObject<List<Dictionary<string, object>>>();
                foreach (Dictionary<string, object> curObj in processData)
                {
                    ProcessWithEachRow(curObj);
                }
            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
            }
            return keyValuePairs;
        }
        private void FetchTheCallDetails(string agentName = "")
        {
            try
            {
                int pageNo = 1;
                string strHasMore = "hasMore";
                Dictionary<string, object> data = ProcessWithResponse(pageNo, agentName);
                while (data != null
                    && data.ContainsKey(strHasMore)
                    && data[strHasMore] is bool
                    && Convert.ToBoolean(data[strHasMore]) == true
                    )
                {
                    pageNo++;
                    data = ProcessWithResponse(pageNo, agentName);
                }
            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
                serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.IssueWhileCallingCallsAPi);
            }
        }
    }
}
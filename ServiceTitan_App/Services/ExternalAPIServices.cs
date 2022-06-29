using ServiceTitan.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ServiceTitan_App.Services
{
    public class ExternalAPIServices : BaseServices
    {
        private readonly ServiceTitanStatusHistoryServices serviceTitanStatusHistoryServices;

        private readonly ErrorHandlerServices errorHandlerServices;
        public ExternalAPIServices()
        {
            serviceTitanStatusHistoryServices = new ServiceTitanStatusHistoryServices();
            errorHandlerServices = new ErrorHandlerServices();
        }

        public void GetServiceTitanJWTAuthentication()
        {
            try
            {
                log.Info("Started GetServiceTitanJWTAuthentication");
                WebClient webClient = new WebClient();
                string serviceTitanTokenURL = ConfigurationManager.AppSettings["ServiceTitanTokenURL"];
                string myParameters = string.Format("client_id={0}&client_secret={1}&grant_type=client_credentials", ClientConstants.STClientID, ClientConstants.STClientSecret);
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string result = webClient.UploadString(serviceTitanTokenURL, myParameters);
                Dictionary<string, object> finalResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                if (finalResponse != null)
                {
                    if (finalResponse.Keys.Contains("access_token"))
                    {
                        ClientConstants.ServiceTitanJWTToken = Convert.ToString(finalResponse["access_token"]);
                    }
                }
                log.Info("End GetServiceTitanJWTAuthentication");
            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
                serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.IssueWhileJWTToken);
            }
        }

        public HttpResponseMessage GetRecording(int Tenant, int LeadCallId, string ServiceTitanJWTToken)
        {
            HttpResponseMessage httpResponseMessage;
            try
            {
                log.Info("GetRecording Called");
                log.Info("Tenant : " + Tenant);
                log.Info("LeadCallId : " + LeadCallId);
                log.Info("ServiceTitanJWTToken : " + ServiceTitanJWTToken);
                HttpClient webClient = new HttpClient();
                string url = ConfigurationManager.AppSettings["ServiceTitanCallsURL"] + "/" + Tenant + "/calls/" + LeadCallId + "/recording";
                webClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ServiceTitanJWTToken);

                string STAppKey = ConfigurationManager.AppSettings["ST_App_Key"];

                webClient.DefaultRequestHeaders.Add("ST-App-Key", STAppKey);
                log.Info("URL : " + url);
                log.Info("STAppKey : " + STAppKey);
                HttpResponseMessage returnResponse = webClient.GetAsync(url).Result;
                log.Info(returnResponse);
                log.Info("GetRecording End");
                return returnResponse;
            }
            catch (Exception ex)
            {
                httpResponseMessage = null;
                errorHandlerServices.HandleError(ex);
                serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.IssueWithDownloadFile);
            }
            return httpResponseMessage;
        }

        public string GetTranscriptFromAssemblyAI(string filePath)
        {
            string transcript = "";
            try
            {
                log.Info("GetTranscriptFromAssemblyAI Called");
                log.Info(filePath);
                HttpClient webClient = new HttpClient();
                string url = ConfigurationManager.AppSettings["AssemblyAIURL"];

                webClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                webClient.DefaultRequestHeaders.Add("authorization", ConfigurationManager.AppSettings["AssemblyAIToken"]);

                AssemblyAIModel assemblyAIModel = new AssemblyAIModel()
                {
                    audio_url = filePath,
                    auto_chapters = true,
                    auto_highlights = true,
                    entity_detection = true,
                    iab_categories = true,
                    sentiment_analysis = true
                };
                log.Info(assemblyAIModel);
                HttpContent httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(assemblyAIModel));

                HttpResponseMessage returnResponse = webClient.PostAsync(url, httpContent).Result;
                if (returnResponse.IsSuccessStatusCode == true)
                {
                    transcript = returnResponse.Content.ReadAsStringAsync().Result;
                }
                log.Info(returnResponse);
                log.Info(transcript);
                log.Info("GetTranscriptFromAssemblyAI completed");
            }
            catch (Exception ex)
            {
                transcript = "";
                errorHandlerServices.HandleError(ex);
                serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.IssueWithTranscribe);
            }
            return transcript;
        }

        public HttpResponseMessage GetCalls(int pageNo, int Tenant, string ServiceTitanJWTToken, string agentName = "")
        {
            HttpResponseMessage httpResponseMessage;
            try
            {
                log.Info("GetCalls called");
                HttpClient webClient = new HttpClient();
                string url = ConfigurationManager.AppSettings["ServiceTitanCallsURL"] + "/" + Tenant + "/calls";

                string STPageSize = ConfigurationManager.AppSettings["STPageSize"];
                string STFromDate = ConfigurationManager.AppSettings["STFromDate"];
                string STToDate = ConfigurationManager.AppSettings["STToDate"];
                string STAppKey = ConfigurationManager.AppSettings["ST_App_Key"];


                webClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ServiceTitanJWTToken);
                webClient.DefaultRequestHeaders.Add("ST-App-Key", STAppKey);

                string myParameters = string.Format("createdAfter={0}&createdBefore={1}&page={2}&pageSize={3}&activeOnly=true", STFromDate, STToDate, pageNo, STPageSize);
                if (!string.IsNullOrWhiteSpace(agentName))
                {
                    myParameters = string.Format("{0}&agentName={1}", myParameters, agentName);
                }
                string finalUrl = string.Format("{0}?{1}", url, myParameters);
                log.Info(finalUrl);
                HttpResponseMessage returnResponse = webClient.GetAsync(finalUrl).Result;
                log.Info("GetCalls completed");
                log.Info(returnResponse);
                return returnResponse;
            }
            catch (Exception ex)
            {
                httpResponseMessage = null;
                errorHandlerServices.HandleError(ex);
                serviceTitanStatusHistoryServices.ManageStatus(StatusEnum.IssueWhileCallingCallsAPi);
            }
            return httpResponseMessage;
        }
    }
}

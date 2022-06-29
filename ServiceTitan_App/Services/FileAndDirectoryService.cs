using ServiceTitan.Core;
using System;
using System.IO;

namespace ServiceTitan_App.Services
{

    public class FileAndDirectoryService : BaseServices
    {
        private readonly ErrorHandlerServices errorHandlerServices;
        public FileAndDirectoryService()
        {
            errorHandlerServices = new ErrorHandlerServices();
        }
        public void CreateDirectory()
        {
            try
            {
                log.Info("CreateDirectory called");
                string directory = GetDirectoryPath();
                if (!Directory.Exists(directory))
                {
                    _ = Directory.CreateDirectory(directory);
                }
                log.Info(directory);
                log.Info("CreateDirectory completed");
            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
            }
        }

        private string GetDirectoryPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "/DownloadedFiles" + "/" + DateTime.UtcNow.ToString("yyyyMMdd") + "/" + ClientConstants.ClientID);
        }
        public void SaveTheFileAsync(int id, Stream ms)
        {
            try
            {
                log.Info("SaveTheFileAsync get called");

                string filePath = GetDirectoryPath() + "/" + id + ".mp3";
                log.Info(filePath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (FileStream outputFileStream = new FileStream(filePath, FileMode.Create))
                {
                    ms.CopyTo(outputFileStream);
                }
                log.Info("SaveTheFileAsync completed");
            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
            }
        }

        public string SaveFileToServerDirectory(int id)
        {
            string returnStr = "http://161.97.104.204/supabearapi/attachments/TestResponse.mp3";
            try
            {
                log.Info("SaveFileToServerDirectory called");
                //todo: Need to write the logic for upload the file on server once client confirm the server directory
                string file = GetDirectoryPath() + "/" + id + ".mp3";
                log.Info("SaveFileToServerDirectory completed");
            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
            }
            return returnStr;
        }
    }
}

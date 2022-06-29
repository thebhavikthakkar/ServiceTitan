using ServiceTitan.Core;
using ServiceTitan.Data;
using System;

namespace ServiceTitan_App.Services
{
    public class CapManagementService : BaseServices
    {
        private readonly ErrorHandlerServices errorHandlerServices;

        public CapManagementService()
        {
            errorHandlerServices = new ErrorHandlerServices();
        }

        private void InsertTheInitialCap(DateTime dt)
        {
            log.Info("InsertTheInitialCap Called");
            log.Info(dt);
            try
            {
                if (ClientConstants.TotalMonthlyCap > 0)
                {
                    double totalMinutes = ClientConstants.TotalMonthlyCap / DateTime.DaysInMonth(dt.Year, dt.Month);
                    log.Info(DateTime.DaysInMonth(dt.Year, dt.Month));
                    log.Info(ClientConstants.TotalMonthlyCap);
                    log.Info(totalMinutes);
                    CapManagement curObj = capManagementHelper.GetByClientIdAndDate(dt, ClientConstants.ClientID);
                    log.Info(curObj);
                    if (curObj == null)
                    {
                        TimeSpan timeSpan = TimeSpan.FromMinutes(totalMinutes);
                        CapManagement model = new CapManagement()
                        {
                            ClientId = ClientConstants.ClientID,
                            date = dt,
                            InitalCap = timeSpan,
                            PendingCap = timeSpan
                        };
                        log.Info(model);
                        capManagementHelper.Insert(model);
                    }
                }
            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
            }
            log.Info("InsertTheInitialCap Called");
        }

        public StatusEnum GetCapStatus(TimeSpan timeSpan, DateTime? date)
        {
            try
            {
                log.Info("GetCapStatus Called");
                log.Info(timeSpan);
                log.Info(date);
                if (date != null)
                {
                    DateTime passDate = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day);
                    InsertTheInitialCap(passDate);
                    CapManagement curObj = capManagementHelper.GetByClientIdAndDate(passDate, ClientConstants.ClientID);
                    log.Info(curObj);
                    if (timeSpan <= curObj.PendingCap)
                    {
                        curObj.PendingCap -= timeSpan;
                        capManagementHelper.Update(curObj);
                        return StatusEnum.HaveEnoughCap;
                    }
                    else
                    {
                        errorHandlerServices.HandleError(new Exception("Not have enough cap."));
                        return StatusEnum.NotHaveEnoughCap;
                    }
                }
                else
                {
                    errorHandlerServices.HandleError(new Exception("date should not be null."));
                    return StatusEnum.NotHaveEnoughCap;
                }

            }
            catch (Exception ex)
            {
                errorHandlerServices.HandleError(ex);
                return StatusEnum.NotHaveEnoughCap;
            }
        }
    }
}

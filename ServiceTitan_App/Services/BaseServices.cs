using log4net;
using ServiceTitan.Data.DBObjects;

namespace ServiceTitan_App.Services
{
   public abstract class BaseServices
    {
        protected readonly ILog log;
        protected readonly ClientMasterHelper clientMaster;
        protected readonly SyncHistoryHelper syncHistory;
        protected readonly ServiceTitanResponseHelper serviceTitanResponseHelper;
        protected readonly CapManagementHelper capManagementHelper;
        protected readonly ErrorHandlerHelper errorHandlerHelper;
        protected readonly ServiceTitanStatusHistory serviceTitanStatusHistory;
        public BaseServices()
        {
            clientMaster = new ClientMasterHelper();
            syncHistory = new SyncHistoryHelper();           
            serviceTitanResponseHelper = new ServiceTitanResponseHelper();
            log = LogManager.GetLogger(typeof(BaseServices));
            capManagementHelper = new CapManagementHelper();
            errorHandlerHelper = new ErrorHandlerHelper();
            serviceTitanStatusHistory = new ServiceTitanStatusHistory();
        }
    }
}

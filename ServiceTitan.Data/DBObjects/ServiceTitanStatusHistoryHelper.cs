namespace ServiceTitan.Data.DBObjects
{
    public class ServiceTitanStatusHistory : DataContext
    {
        public void Insert(ServiceStatusHistory serviceStatusHistory)
        {
            _ = dbContext.ServiceStatusHistories.Add(serviceStatusHistory);
            _ = dbContext.SaveChanges();
        }
    }
}

namespace ServiceTitan.Data.DBObjects
{
    public class ErrorHandlerHelper : DataContext
    {
        public void Insert(ErrorHandler errorHandler)
        {
            _ = dbContext.ErrorHandlers.Add(errorHandler);
            _ = dbContext.SaveChanges();
        }
    }
}

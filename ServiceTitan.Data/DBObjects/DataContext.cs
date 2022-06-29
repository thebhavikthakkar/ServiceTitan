namespace ServiceTitan.Data.DBObjects
{
   public abstract class DataContext
    {
        protected readonly RyanStomelEntities dbContext;

        public DataContext()
        {
            dbContext = new RyanStomelEntities();
        }
    }
}

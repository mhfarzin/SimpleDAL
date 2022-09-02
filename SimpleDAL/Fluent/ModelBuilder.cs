namespace SimpleDAL
{
    public class ModelBuilder
    {
        public FluentConfig<TEntity> Entity<TEntity>()
        {
            return new FluentConfig<TEntity>();
        }
    }
}

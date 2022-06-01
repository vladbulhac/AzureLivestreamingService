namespace Livestreaming.Infrastructure.Repositories
{
    public interface IRepository<TEntity>
    {
        public Task<TEntity> GetAsync(string id);

        public Task<ICollection<TEntity>> GetAllAsync(string id, int page);

        public Task AddAsync(TEntity entity);

        public Task SaveEntitiesChangesAsync();

        public Task DeleteAsync(string id);
    }
}
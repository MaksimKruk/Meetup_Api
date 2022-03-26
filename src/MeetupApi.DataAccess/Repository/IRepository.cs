namespace MeetupApi.DataAccess.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetItemAsync(int id);
        Task PutItemAsync(TEntity item);
        Task PostItemAsync(TEntity item);
        Task DeleteItemAsync(int id);
        bool ItemExists(int id);
    }
}

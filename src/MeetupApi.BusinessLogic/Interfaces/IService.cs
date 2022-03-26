namespace MeetupApi.BusinessLogic.Interfaces
{
    public interface IService<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetItemAsync(int id);
        Task PutItemAsync(int id, TEntity item);
        Task PostItemAsync(TEntity item);
        Task DeleteItemAsync(int id);
        bool EntityExists(int id);
    }
}

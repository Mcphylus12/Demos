namespace BE_CustomerStore.Data
{
    public interface IStore<T>
        where T : class, IEntity
    {
        Task<IEnumerable<T>> Get();
        Task<IEnumerable<T>> Get(IQuery<T> query);
        Task<Guid> Add(T newItem);
    }
}

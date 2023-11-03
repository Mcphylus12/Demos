namespace BE_CustomerStore.Data
{
    public class MemoryStore<T> : IStore<T>
        where T : class, IEntity
    {
        private readonly Dictionary<Guid, T> _data;

        public MemoryStore()
        {
            _data = new Dictionary<Guid, T>();
        }

        public Task<Guid> Add(T newItem)
        {
            newItem.Id = Guid.NewGuid();
            _data.Add(newItem.Id, newItem);
            return Task.FromResult(newItem.Id);
        }

        public Task<IEnumerable<T>> Get()
        {
            return Task.FromResult(_data.Values.AsEnumerable());
        }

        public Task<IEnumerable<T>> Get(IQuery<T> query)
        {
            return Task.FromResult(query.Specify(_data.Values.AsQueryable()).AsEnumerable());
        }
    }
}

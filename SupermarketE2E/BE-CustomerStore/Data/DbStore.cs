using Microsoft.EntityFrameworkCore;

namespace BE_CustomerStore.Data
{
    public class DbStore<T> : IStore<T>
        where T : class, IEntity
    {
        private readonly DatabaseContext _context;

        public DbStore(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Guid> Add(T newItem)
        {
            newItem.Id = Guid.NewGuid();
            _context.Set<T>().Add(newItem);
            await _context.SaveChangesAsync();
            return newItem.Id;
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> Get(IQuery<T> query)
        {
            return await query.Specify(_context.Set<T>()).ToListAsync();
        }
    }
}

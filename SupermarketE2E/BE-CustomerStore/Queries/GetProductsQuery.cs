using BE_CustomerStore.Data;
using BE_CustomerStore.Modelling;

namespace BE_CustomerStore.Queries;

public class GetProductsQuery : IQuery<Product>
{
    private readonly Guid? _category;

    public GetProductsQuery(Guid? category)
    {
        _category = category;
    }

    public IQueryable<Product> Specify(IQueryable<Product> query)
    {
        if (_category.HasValue)
        {
            query = query.Where(p => p.CategoryId == _category.Value);
        }

        return query;
    }
}

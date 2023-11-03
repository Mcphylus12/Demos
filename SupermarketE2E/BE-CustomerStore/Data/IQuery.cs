namespace BE_CustomerStore.Data;

public interface IQuery<TData>
{
    IQueryable<TData> Specify(IQueryable<TData> query);
}

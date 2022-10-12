namespace Endava.BookSharing.Application.Abstract;

public abstract class AbstractHandler<T> where T : class
{
    protected AbstractHandler<T> successor = null!;
    public string Data { get; set; } = null!;

    public void SetSuccessor(AbstractHandler<T> successor)
    {
        this.successor = successor;
    }

    public abstract Task ProcessRequestAsync(T entity, CancellationToken cancellationToken);
}
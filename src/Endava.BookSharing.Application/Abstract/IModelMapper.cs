namespace Endava.BookSharing.Application.Abstract;

public interface IModelMapper
{
    TResult Map<TResult>(object source);
}
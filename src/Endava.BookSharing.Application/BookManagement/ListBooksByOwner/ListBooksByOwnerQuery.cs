using Endava.BookSharing.Application.Filters;
using Endava.BookSharing.Domain.Entities.Pagination;
using MediatR;

namespace Endava.BookSharing.Application.BookManagement.ListBooksByOwner;

public class ListBooksByOwnerQuery : IRequest<PaginationList<ListBooksByOwnerItemsResponse>>
{
    public ListBooksByOwnerQuery(string ownerId, PaginationFilter filter)
    {
        OwnerId = ownerId;
        Filter = filter;
    }

    public string OwnerId { get; }
    public PaginationFilter Filter { get; }
}
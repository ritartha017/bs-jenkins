using Endava.BookSharing.Domain.Entities;
using MediatR;

namespace Endava.BookSharing.Application.GenresManagement.GetGenresList;

public class GetGenresListQuery : IRequest<List<Genre>>
{
}
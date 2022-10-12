using Endava.BookSharing.Domain.Entities;
using MediatR;

namespace Endava.BookSharing.Application.LanguageManagement.GetLanguagesList;

public class GetLanguagesListQuery : IRequest<List<Language>>
{
}
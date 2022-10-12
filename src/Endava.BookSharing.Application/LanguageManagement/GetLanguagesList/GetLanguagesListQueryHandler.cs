using Endava.BookSharing.Application.Abstract;
using Endava.BookSharing.Application.Exceptions;
using Endava.BookSharing.Domain.Entities;
using MediatR;

namespace Endava.BookSharing.Application.LanguageManagement.GetLanguagesList;

public class GetLanguagesListQueryHandler : IRequestHandler<GetLanguagesListQuery, List<Language>>
{
    private readonly ILanguageRepository _languageRepository;

    public GetLanguagesListQueryHandler(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public async Task<List<Language>> Handle(GetLanguagesListQuery request, CancellationToken cancellationToken)
    {
        var languages = await _languageRepository.ListAllAsync(cancellationToken);

        if (languages is null)
        {
            throw new BookSharingInternalException();
        }

        return languages;
    }
}
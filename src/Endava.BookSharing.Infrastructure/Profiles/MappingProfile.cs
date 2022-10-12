using AutoMapper;
using Endava.BookSharing.Application.Models.Response;
using Endava.BookSharing.Domain.Entities;
using Endava.BookSharing.Infrastructure.Persistence.Identity.Models;
using Endava.BookSharing.Infrastructure.Persistence.Models;

namespace Endava.BookSharing.Infrastructure.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Language, LanguageDb>().ReverseMap();
        CreateMap<Author, AuthorDb>().ReverseMap();
        CreateMap<Genre, GenreDb>().ReverseMap();
        CreateMap<AuthorResponse, AuthorDb>().ReverseMap();
        CreateMap<UserResponse, UserDb>().ReverseMap();
        CreateMap<Language, LanguageDb>().ReverseMap();
        CreateMap<Book, BookDb>().ReverseMap();
    }
}
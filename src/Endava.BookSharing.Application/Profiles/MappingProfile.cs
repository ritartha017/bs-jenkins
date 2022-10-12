using AutoMapper;
using Endava.BookSharing.Application.AuthorManagement.GetAuthorsList;
using Endava.BookSharing.Application.Models.DtoModels;
using Endava.BookSharing.Application.Models.Response;
using Endava.BookSharing.Domain.Entities;

namespace Endava.BookSharing.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Author, GetAuthorsListItemResponse>().ReverseMap();
        CreateMap<BookDetailsDto, BookResponse>().ReverseMap();
    }
}
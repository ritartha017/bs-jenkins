using AutoMapper;

namespace Endava.BookSharing.Application.UnitTests.Profiles;

public class MappingTestsFixture
{
    public MappingTestsFixture()
    {
        ConfigurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<Application.Profiles.MappingProfile>();
        });

        Mapper = ConfigurationProvider.CreateMapper();
    }

    public IConfigurationProvider ConfigurationProvider { get; }
    public IMapper Mapper { get; }
}
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endava.BookSharing.Presentation.UnitTests.Shared.Models;
public class FormData : IFormFile
{
    public string ContentType { get; } = "test";
    public string ContentDisposition { get; } = "test";
    public IHeaderDictionary Headers { get; } = null!;
    public long Length { get; } = 1;
    public string Name { get; } = "test";
    public string FileName { get; } = "test";
    public void CopyTo(Stream target) { /*nothing*/ }
    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) { return Task.CompletedTask; }
    public Stream OpenReadStream() { return Stream.Null; }
}
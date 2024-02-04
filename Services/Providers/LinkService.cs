using Template.MinimalApi.NET8.Models;
using Template.MinimalApi.NET8.Services.Interfaces;

namespace Template.MinimalApi.NET8.Services.Providers;

internal sealed class LinkService : ILinkService
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LinkService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }

    public Link GenerateLink(string endpointName, object? parameters, string rel, string methodName)
    {
        return new Link
        (
            Href: _linkGenerator.GetUriByName(_httpContextAccessor.HttpContext!, endpointName, parameters),
            Rel: rel,
            Method: methodName
        );
    }
}
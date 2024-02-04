using Template.MinimalApi.NET8.Models;

namespace Template.MinimalApi.NET8.Services.Interfaces;

public interface ILinkService
{
    Link GenerateLink(string endpointName, object? parameters, string rel, string methodName);
}
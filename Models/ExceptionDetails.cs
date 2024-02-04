namespace Template.MinimalApi.NET8.Models;

#pragma warning disable CS8618

public class ExceptionDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Details { get; set; }
}
#pragma warning disable CS8618

namespace Template.MinimalApi.NET8.Options;

public class DatabaseConfig
{
    public string DbConnectionString { get; set; }
    public bool EnableRetryOnFailure { get; set; }
    public int MaxRetryCount { get; set; }
    public int MaxRetryDelay { get; set; }
    public ICollection<int> ErrorNumbersToAdd { get; set; }
    public int CommandTimeout { get; set; }
}
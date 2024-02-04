namespace Template.MinimalApi.NET8.Repositories.Interfaces;

public interface IPgRepository
{
    Task<int> SaveChangesAsync();
}
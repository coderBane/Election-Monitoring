namespace Election2023.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : class;

    Task<int> CommitAsync();

    Task RollBack();

}


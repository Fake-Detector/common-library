namespace Common.Library.Queue.InMemoryQueue.Interfaces;

public interface IInMemoryQueue<T>
{
    public Task WriteAsync(T item);

    public Task<T> ReadAsync();
}
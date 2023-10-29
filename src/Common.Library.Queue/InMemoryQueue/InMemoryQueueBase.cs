using System.Threading.Channels;
using Common.Library.Queue.InMemoryQueue.Configuration;
using Common.Library.Queue.InMemoryQueue.Interfaces;

namespace Common.Library.Queue.InMemoryQueue;

public abstract class InMemoryQueueBase<T> : IInMemoryQueue<T>
{
    private readonly Channel<T> _channel;

    protected InMemoryQueueBase(InMemoryQueueOptions options) =>
        _channel = Channel.CreateBounded<T>(new BoundedChannelOptions(options.Capacity)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait
        });

    public async Task WriteAsync(T item) => await _channel.Writer.WriteAsync(item);

    public async Task<T> ReadAsync() => await _channel.Reader.ReadAsync();
}
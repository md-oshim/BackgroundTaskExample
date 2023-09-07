using BackgroundTaskExample.Models;
using System.Threading.Channels;

namespace BackgroundTaskExample.Services
{
    public interface IDocumentationQueue
    {
        ValueTask Enqueue(RequestModel workItem);
        ValueTask<RequestModel> Dequeue(CancellationToken cancellationToken);
    }

    public class DocumentationQueue : IDocumentationQueue
    {
        private readonly Channel<RequestModel> _queue;

        public DocumentationQueue(int capacity = 100)
        {
            // Capacity should be set based on the expected application load and
            // number of concurrent threads accessing the queue.            
            // BoundedChannelFullMode.Wait will cause calls to WriteAsync() to return a task,
            // which completes only when space became available. This leads to backpressure,
            // in case too many publishers/calls start accumulating.
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<RequestModel>(options);

        }

        public async ValueTask Enqueue(RequestModel workItem)
        {
            await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask<RequestModel> Dequeue(CancellationToken cancellationToken)
        {
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);

            return workItem;
        }
    }
}

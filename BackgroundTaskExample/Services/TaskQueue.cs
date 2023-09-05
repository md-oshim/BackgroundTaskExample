using BackgroundTaskExample.Models;
using System.Threading.Channels;

namespace BackgroundTaskExample.Services
{
    public interface ITaskQueue
    {
        public Task QueueBackgroundWorkItemAsync(RequestModel requestModel);

        Task<RequestModel> DequeueAsync();

        bool HasTask();
    }

    public class TaskQueue : ITaskQueue
    {
        private readonly Channel<RequestModel> _queue;

        public TaskQueue(int capacity = 10)
        {
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<RequestModel>(options);
        }

        public async Task QueueBackgroundWorkItemAsync(RequestModel workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            await _queue.Writer.WriteAsync(workItem);
        }

        public async Task<RequestModel> DequeueAsync()
        {
            var workItem = await _queue.Reader.ReadAsync(CancellationToken.None);

            return workItem;
        }

        public bool HasTask()
        {
            return _queue.Reader.Count > 0;
        }

    }

}


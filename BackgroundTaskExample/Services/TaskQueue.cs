using BackgroundTaskExample.Models;

namespace BackgroundTaskExample.Services
{
    public class TaskQueue
    {
        private readonly Queue<RequestModel> _queue = new();
        public static readonly SemaphoreSlim Signal = new SemaphoreSlim(0);

        public void Enqueue(RequestModel requestModel)
        {
            _queue.Enqueue(requestModel);
            Signal.Release();
        }

        public RequestModel Dequeue()
        {
            return _queue.Dequeue();
        }

    }

}


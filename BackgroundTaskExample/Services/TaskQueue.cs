using BackgroundTaskExample.Models;

namespace BackgroundTaskExample.Services
{
    public class TaskQueue
    {
        private readonly Queue<RequestModel> _queue = new();

        public void Enqueue(RequestModel requestModel)
        {
            _queue.Enqueue(requestModel);
        }

        public RequestModel Dequeue()
        {
            return _queue.Dequeue();
        }

        public bool HasTask()
        {
            return _queue.Count > 0;
        }

    }

}


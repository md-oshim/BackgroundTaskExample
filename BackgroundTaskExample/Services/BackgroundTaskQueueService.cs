using BackgroundTaskExample.Models;

namespace BackgroundTaskExample.Services
{
    public class BackgroundTaskQueueService : BackgroundService
    {
        public RequestModel _requestModel = new RequestModel();
        public ITaskQueue _taskQueue { get; }

        public BackgroundTaskQueueService(ITaskQueue taskQueue)
        {
            _taskQueue = taskQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_taskQueue.HasTask())
                {
                    _requestModel = await _taskQueue.DequeueAsync();

                    int executionDuration = _requestModel.TaskTimeInSeconds;

                    Console.WriteLine($"Queued, Starting execution of TaskId: {_requestModel?.TaskId}, TaskDuration: {executionDuration} seconds");

                    await Task.Delay(1000 * executionDuration, stoppingToken);

                    Console.WriteLine($"Completed queued execution of TaskId: {_requestModel?.TaskId}");
                }
                else
                {
                    await Task.Delay(1000);
                }
            }
        }

    }

}


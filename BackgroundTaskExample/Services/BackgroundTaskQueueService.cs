using BackgroundTaskExample.Models;

namespace BackgroundTaskExample.Services
{
    public class BackgroundTaskQueueService : BackgroundService
    {
        public RequestModel _requestModel = new();
        public TaskQueue _taskQueue;
        public BackgroundTaskResponseHub _BackgroundTaskResponseHub;
        public BackgroundTaskQueueService(
            TaskQueue taskQueue,
            BackgroundTaskResponseHub backgroundTaskResponseHub)
        {
            _taskQueue = taskQueue;
            _BackgroundTaskResponseHub = backgroundTaskResponseHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await TaskQueue.Signal.WaitAsync(stoppingToken);

                _requestModel = _taskQueue.Dequeue();

                int executionDuration = _requestModel.TaskTimeInSeconds;
                Console.WriteLine($"Queued, Starting execution of TaskId: {_requestModel?.TaskId}, TaskDuration: {executionDuration} seconds");

                await Task.Delay(1000 * executionDuration, stoppingToken);

                Console.WriteLine($"Completed queued execution of TaskId: {_requestModel?.TaskId}");
                await _BackgroundTaskResponseHub.SendMessageQueueTask(_requestModel.TaskId, _requestModel.ConnectionId);

            }
        }

    }

}


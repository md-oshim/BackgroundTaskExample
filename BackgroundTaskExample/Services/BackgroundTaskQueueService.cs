using BackgroundTaskExample.Models;

namespace BackgroundTaskExample.Services
{
    public class BackgroundTaskQueueService : BackgroundService
    {
        public RequestModel _requestModel = new();
        private readonly IDocumentationQueue _documentationQueue;

        public BackgroundTaskResponseHub _BackgroundTaskResponseHub;
        public BackgroundTaskQueueService(
            IDocumentationQueue documentationQueue,
            BackgroundTaskResponseHub backgroundTaskResponseHub)
        {
            _documentationQueue = documentationQueue;
            _BackgroundTaskResponseHub = backgroundTaskResponseHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var workItem = await _documentationQueue.Dequeue(stoppingToken);

                    try
                    {
                        _requestModel = workItem;

                        int executionDuration = _requestModel.TaskTimeInSeconds;
                        Console.WriteLine($"Queued, Starting execution of TaskId: {_requestModel?.TaskId}, TaskDuration: {executionDuration} seconds");

                        await Task.Delay(1000 * executionDuration, stoppingToken);

                        Console.WriteLine($"Completed queued execution of TaskId: {_requestModel?.TaskId}");
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
        }

    }

}


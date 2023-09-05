using BackgroundTaskExample.Models;

namespace BackgroundTaskExample.Services
{
    public class BackgroundTaskParallelService : BackgroundService
    {
        public RequestModel _requestModel = new RequestModel();
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int executionDuration = _requestModel.TaskTimeInSeconds;

            Console.WriteLine($"Parallel, Starting execution of TaskId: {_requestModel?.TaskId}, TaskDuration: {executionDuration} seconds");

            await Task.Delay(1000 * executionDuration, stoppingToken);

            Console.WriteLine($"Completed parallel execution of TaskId: {_requestModel?.TaskId}");
        }

        public async Task StartParallelBackgroundTask(RequestModel requestModel)
        {
            _requestModel = requestModel;

            await ExecuteAsync(CancellationToken.None);
        }

    }

}


using BackgroundTaskExample.Models;

namespace BackgroundTaskExample.Services
{
    public class BackgroundTaskParallelService : BackgroundService
    {
        public RequestModel _requestModel = new();
        private readonly BackgroundTaskResponseHub _backgroundTaskResponseHub;

        public BackgroundTaskParallelService(BackgroundTaskResponseHub backgroundTaskResponseHub)
        {
            _backgroundTaskResponseHub = backgroundTaskResponseHub;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int executionDuration = _requestModel.TaskTimeInSeconds;

            Console.WriteLine($"Parallel, Starting execution of TaskId: {_requestModel?.TaskId}, TaskDuration: {executionDuration} seconds");

            await Task.Delay(1000 * executionDuration, stoppingToken);

            Console.WriteLine($"Completed parallel execution of TaskId: {_requestModel?.TaskId}");

            await _backgroundTaskResponseHub.SendMessageParallelTask(_requestModel.TaskId, _requestModel.ConnectionId);

        }

        public async Task StartParallelBackgroundTask(RequestModel requestModel)
        {
            _requestModel = requestModel;

            await ExecuteAsync(CancellationToken.None);
        }

    }

    public interface IBackgroundTaskParallelService
    {
        Task StartParallelBackgroundTask(RequestModel requestModel);
    }

}


using BackgroundTaskExample.Models;
using BackgroundTaskExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundTaskExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackgroundTaskController : ControllerBase
    {
        private readonly BackgroundTaskParallelService _backgroundTaskParallelService;
        private readonly IDocumentationQueue _documentationQueue;
        private readonly ILogger<BackgroundTaskController> _logger;

        //private readonly TaskQueue _taskQueue;

        public BackgroundTaskController(
            BackgroundTaskParallelService backgroundTaskParallelService,
            //TaskQueue taskQueue
            IDocumentationQueue documentationQueue,
            ILogger<BackgroundTaskController> logger)
        {
            _backgroundTaskParallelService = backgroundTaskParallelService;
            //_taskQueue = taskQueue;
            _documentationQueue = documentationQueue;
            _logger = logger;
        }

        [HttpGet("parallel")]
        public ActionResult Parallel([FromQuery] RequestModel requestModel)
        {
            _ = _backgroundTaskParallelService.StartParallelBackgroundTask(requestModel);

            return Ok(requestModel.TaskId);
        }

        [HttpGet("queue")]
        public async Task<ActionResult> Queue([FromQuery] RequestModel requestModel)
        {
            await _documentationQueue.Enqueue(requestModel);

            return Ok(requestModel.TaskId);
        }


        private async ValueTask BuildWorkItem(CancellationToken token)
        {
            // Simulate three 5-second tasks to complete
            // for each enqueued work item

            int delayLoop = 0;
            var guid = Guid.NewGuid().ToString();

            while (!token.IsCancellationRequested && delayLoop < 3)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), token);
                }
                catch (OperationCanceledException)
                {
                    // Prevent throwing if the Delay is cancelled
                }

                delayLoop++;

                _logger.LogInformation("Queued Background Task {Guid} is running. "
                                       + "{DelayLoop}/3", guid, delayLoop);
            }

            if (delayLoop == 3)
            {
                _logger.LogInformation("Queued Background Task {Guid} is complete.", guid);
            }
            else
            {
                _logger.LogInformation("Queued Background Task {Guid} was cancelled.", guid);
            }
        }
    }

}


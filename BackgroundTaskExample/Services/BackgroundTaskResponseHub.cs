using Microsoft.AspNetCore.SignalR;

namespace BackgroundTaskExample.Services
{
    public class BackgroundTaskResponseHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ClientConnected");
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageQueueTask(int taskId, string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("taskCompleted", $"Queued, TaskId: {taskId} has been completed");
        }

        public async Task SendMessageParallelTask(int taskId, string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("taskCompleted", $"Parallel, TaskId: {taskId} has been completed");
        }
    }
}

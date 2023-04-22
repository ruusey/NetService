namespace NetService.Async
{
    public interface IScopedService
    {
        Task DoWorkAsync(CancellationToken stoppingToken);

    }
}

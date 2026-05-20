using NoExcusesFit.Domain.Interfaces.Repositories;

namespace NoExcusesFit.Worker
{
    public class Worker: BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at {time}", DateTimeOffset.Now);

                using var scope = _serviceScopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();

                int rowsDeleted = await repository.DeleteExpiredAsync();
                _logger.LogInformation("Linhas afetadas: {RowsAffected}", rowsDeleted);

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}

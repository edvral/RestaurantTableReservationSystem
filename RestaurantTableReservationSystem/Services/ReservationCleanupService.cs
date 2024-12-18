using RestaurantTableReservationSystem.Data;

namespace RestaurantTableReservationSystem.Services
{
    public class ReservationCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ReservationCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<RestaurantReservationContext>();

                    var now = DateTime.Now;
                    var expiredReservations = dbContext.Reservations
                        .Where(r => r.ReservationEnd < now)
                        .ToList();

                    if (expiredReservations.Any())
                    {
                        dbContext.Reservations.RemoveRange(expiredReservations);
                        await dbContext.SaveChangesAsync();
                    }
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}

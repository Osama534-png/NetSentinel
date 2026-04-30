using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetSentinel.Data;
using NetSentinel.Models;
using System.Diagnostics;

namespace NetSentinel.Services
{
    public class PingWorkerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PingWorkerService> _logger;

        public PingWorkerService(IServiceScopeFactory scopeFactory, IHttpClientFactory httpClientFactory, ILogger<PingWorkerService> logger)
        {
            _scopeFactory = scopeFactory;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("NetSentinel Ping Motoru çalışıyor: {time}", DateTimeOffset.Now);

                //  DbContext "Scoped" olduğu için Singleton olan bu serviste "Scope" açmalıyız!
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<NetSentinelDbContext>();

                    // Sadece aktif olarak işaretlenen siteleri getir
                    var websites = dbContext.Websites.Where(w => w.IsActive).ToList();

                    // IHttpClientFactory kullanarak en iyi pratiklerle HTTP isteği atıyoruz
                    var httpClient = _httpClientFactory.CreateClient();

                    foreach (var site in websites)
                    {
                        var log = new HealthLog { WebsiteId = site.Id };
                        var stopwatch = Stopwatch.StartNew();

                        try
                        {
                            // Siteye GET isteği atıyoruz
                            var response = await httpClient.GetAsync(site.Url, stoppingToken);
                            stopwatch.Stop();

                            log.StatusCode = (int)response.StatusCode;
                            log.IsUp = response.IsSuccessStatusCode;
                            log.ResponseTime = stopwatch.ElapsedMilliseconds;
                        }
                        catch (Exception ex)
                        {
                            // Site çökmüşse veya URL hatalıysa buraya düşer
                            stopwatch.Stop();
                            log.IsUp = false;
                            log.ErrorMessage = ex.Message;
                            log.ResponseTime = stopwatch.ElapsedMilliseconds;
                        }

                        dbContext.HealthLogs.Add(log);
                        site.LastCheckTime = DateTime.Now;
                    }

                    // Tüm logları tek seferde veritabanına kaydet
                    await dbContext.SaveChangesAsync(stoppingToken);
                }

                // Test için 1 dakikada bir çalışsın ()
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}

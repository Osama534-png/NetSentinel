using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetSentinel.Data;
using NetSentinel.Models;

namespace NetSentinel.Controllers
{
    public class HomeController : Controller
    {
        private readonly NetSentinelDbContext _context;

        // Veritabanını Controller'a bağlıyoruz (Dependency Injection)
        public HomeController(NetSentinelDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Tüm siteleri getir
            var websites = await _context.Websites.ToListAsync();

            // Her site için veritabanındaki en son (en güncel) ping logunu buluyoruz
            var latestLogs = new Dictionary<int, HealthLog>();
            foreach (var site in websites)
            {
                var log = await _context.HealthLogs
                                        .Where(l => l.WebsiteId == site.Id)
                                        .OrderByDescending(l => l.CheckDate)
                                        .FirstOrDefaultAsync();
                if (log != null)
                {
                    latestLogs.Add(site.Id, log);
                }
            }

            // Logları View (Arayüz) tarafına taşıyabilmek için ViewBag içine atıyoruz
            ViewBag.LatestLogs = latestLogs;

            return View(websites);
        }

        [HttpPost]
        public async Task<IActionResult> AddWebsite(string name, string url)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(url))
            {
                var newSite = new Website
                {
                    Name = name,
                    Url = url,
                    IsActive = true,
                    LastCheckTime = DateTime.Now
                };

                _context.Websites.Add(newSite);
                await _context.SaveChangesAsync();
            }

            // Siteyi ekledikten sonra ana sayfaya geri dön
            return RedirectToAction("Index");
        }
    }
}
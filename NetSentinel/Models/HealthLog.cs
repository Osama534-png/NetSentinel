using Microsoft.AspNetCore.Mvc;

namespace NetSentinel.Models
{
    public class HealthLog
    {
        public int Id { get; set; }
        public int WebsiteId { get; set; }
        public int StatusCode { get; set; } // 200, 404, 500 gibi
        public bool IsUp { get; set; }      // Erişilebilir mi?
        public long ResponseTime { get; set; } // Kaç ms sürdü?
        public DateTime CheckDate { get; set; } = DateTime.Now;
        public string? ErrorMessage { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace NetSentinel.Models
{
    public class Website
    {
        public int Id { get; set; }
        public string Name { get; set; } // Örn: Google
        public string Url { get; set; }  // Örn: https://www.google.com
        public bool IsActive { get; set; } = true;
        public DateTime LastCheckTime { get; set; }
    }
}

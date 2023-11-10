using System.ComponentModel.DataAnnotations.Schema;

namespace JRSystem.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string? JobId { get; set; }   
        public string Title { get; set; }
        public string Company { get; set; }
        public string Job_type { get; set; }
        public string? Job_description { get; set; }
        public DateTime? Start_time { get; set; }

    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace JRSystem.Models
{
    public class Application
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationId { get; set; }

        public string Summary { get; set; }
        public int? ApplierId { get; set; }
        public string? ReferralId { get; set; }
        public string? FileId { get; set; }

    }
}

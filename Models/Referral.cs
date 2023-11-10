namespace JRSystem.Models
{
    public class Referral
    {
        public int ReferralId { get; set; } 
        public string ReferralName { get; set; }
        public int? AccountID { get; set; }
        public DateTime? ReferralDate { get; set; }
        public DateTime deadline { get; set; }
        public string JobTitle { get; set; }
        public int Num_seats { get; set; }
        public string? JobId { get; set; }


    }
}

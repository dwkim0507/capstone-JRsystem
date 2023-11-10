namespace JRSystem.Models
{
    public class File
    {
        public int Id { get; set; }
        public string? FileId { get; set; }
        public int? UserId { get; set; }
        public string? ReferralId { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}

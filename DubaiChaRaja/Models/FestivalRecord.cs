namespace DubaiChaRaja.Models
{
    public class FestivalRecord
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
    }
}
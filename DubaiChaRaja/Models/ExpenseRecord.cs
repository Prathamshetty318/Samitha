namespace Dubaicharaja.Models
{
    public class ExpenseRecord
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }  //income or exp
        public string Description { get; set; }
        public decimal Amount { get; set; }

    }
}

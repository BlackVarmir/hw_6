namespace hw_6.Models
{
    public class Movie
    {
        public string PartitionKey { get; set; } 
        public string RowKey { get; set; } 
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public double Rating { get; set; }
    }
}

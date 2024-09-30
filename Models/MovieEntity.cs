using Azure;
using Azure.Data.Tables;

namespace hw_6.Models
{
    public class MovieEntity : ITableEntity
    {
        public string PartitionKey { get; set; }  
        public string RowKey { get; set; }        
        public string Title { get; set; }         
        public string Genre { get; set; }         
        public int Year { get; set; }             
        public string Director { get; set; }      
        public double Rating { get; set; }        
        public DateTimeOffset? Timestamp { get; set; } 
        public ETag ETag { get; set; }
    }
}

using Azure;
using Azure.Data.Tables;
using hw_6.Models;

namespace hw_6.Services
{
    public class MovieService
    {
        private readonly TableClient _tableClient;

        public MovieService(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("AzureTableStorage:ConnectionString").Value;
            _tableClient = new TableClient(connectionString, "Movies");
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            var movies = new List<Movie>();
            var entities = _tableClient.Query<MovieEntity>();

            foreach (var entity in entities)
            {
                movies.Add(new Movie
                {
                    PartitionKey = entity.PartitionKey,
                    RowKey = entity.RowKey,
                    Title = entity.Title,
                    Genre = entity.Genre,
                    Year = entity.Year,
                    Director = entity.Director,
                    Rating = entity.Rating
                });
            }

            return movies;
        }

        public async Task<Movie> GetMovieAsync(string partitionKey, string rowKey)
        {
            try
            {
                var entity = await _tableClient.GetEntityAsync<MovieEntity>(partitionKey, rowKey);
                return new Movie
                {
                    PartitionKey = entity.Value.PartitionKey,
                    RowKey = entity.Value.RowKey,
                    Title = entity.Value.Title,
                    Genre = entity.Value.Genre,
                    Year = entity.Value.Year,
                    Director = entity.Value.Director,
                    Rating = entity.Value.Rating
                };
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null; // Якщо фільм не знайдено, повертаємо null
            }
        }


        public async Task CreateMovieAsync(Movie movie)
        {
            var entity = new MovieEntity
            {
                PartitionKey = movie.PartitionKey,
                RowKey = Guid.NewGuid().ToString() /*movie.RowKey*/,
                Title = movie.Title,
                Genre = movie.Genre,
                Year = movie.Year,
                Director = movie.Director,
                Rating = movie.Rating
            };
            await _tableClient.AddEntityAsync(entity);
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            var entity = new MovieEntity
            {
                PartitionKey = movie.PartitionKey,
                RowKey = movie.RowKey,
                Title = movie.Title,
                Genre = movie.Genre,
                Year = movie.Year,
                Director = movie.Director,
                Rating = movie.Rating,
                ETag = ETag.All
            };
            await _tableClient.UpdateEntityAsync(entity, ETag.All, TableUpdateMode.Replace);
        }

        public async Task DeleteMovieAsync(string partitionKey, string rowKey)
        {
            await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<List<Movie>> GetMoviesByYearAsync(int year)
        {
            var movies = _tableClient.Query<MovieEntity>(e => e.Year == year);
            return movies.Select(e => new Movie
            {
                PartitionKey = e.PartitionKey,
                RowKey = e.RowKey,
                Title = e.Title,
                Genre = e.Genre,
                Year = e.Year,
                Director = e.Director,
                Rating = e.Rating
            }).ToList();
        }
    }
}

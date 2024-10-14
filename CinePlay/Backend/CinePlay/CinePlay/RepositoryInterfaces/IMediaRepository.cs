using MyMDb.Models;

namespace MyMDb.RepositoryInterfaces
{
    public interface IMediaRepository : IRepository<Media>
    {
        // create
        Task<Movie> CreateMovieAsync(Movie movie);
        Task<Series> CreateSeriesAsync(Series series);
        Task<Episode> CreateEpisodeAsync(Episode episode);
        
        // get all
        Task<ICollection<Media>> GetAllMediaAsync();
        Task<ICollection<Series>> GetAllSeriesAsync();
        Task<ICollection<Movie>> GetAllMoviesAsync();
        Task<ICollection<Episode>> GetEpisodesBySeriesIdAsync(Guid seriesId);

        // get by id
        Task<Movie?> GetMovieByIdAsync(Guid id);

        Task<Series?> GetSeriesByIdAsync(Guid id);

        Task<Episode?> GetEpisodeByIdAsync(Guid id);

        // get reviews
        Task<ICollection<Review>> GetReviewsByIdAsync(Guid Id);

        // update
        Task UpdateMovie(Movie movie);
        Task UpdateSeries(Series series);
        Task UpdateEpisode(Episode episode);

        // others
        Task<double?> GetAverageReviewScoreAsync(Guid Id);
        Task<int> GetLastEpisodeOfASeasonAsync(Guid seriesId, int seasonNumber);
    }
}

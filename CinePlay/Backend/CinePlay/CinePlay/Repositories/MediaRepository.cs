using Microsoft.EntityFrameworkCore;
using MyMDb.Data;
using MyMDb.Models;
using MyMDb.RepositoryInterfaces;

namespace MyMDb.Repositories
{
    public class MediaRepository : Repository<Media>, IMediaRepository
    {
        public MediaRepository (ApplicationDbContext context) : base(context) 
        {

        }

        public async Task<ICollection<Review>> GetReviewsByIdAsync(Guid Id)
        {
            return await _context.Reviews
                .Where(r => r.MediaId == Id)
                .OrderByDescending(r => r.DateCreated)
                .ToListAsync();
        }

        public async Task<double?> GetAverageReviewScoreAsync(Guid Id)
        {
            var reviews = await _context.Reviews
                .Where(r => r.MediaId == Id)
                .ToListAsync();

            if (reviews.Count == 0)
                return null;

            return reviews.Average(r => r.Rating);
        }

        public async Task<ICollection<Episode>> GetEpisodesBySeriesIdAsync(Guid seriesId)
        {
            return await _dbSet
                .OfType<Episode>()
                .Where(e => e.SeriesId == seriesId)
                .OrderBy(e => e.SeasonNumber)
                .ThenBy(e =>  e.EpisodeNumber)
                .ToListAsync();
        }

        public async Task<ICollection<Media>> GetAllMediaAsync()
        {
            var movies = await _dbSet.OfType<Movie>().ToListAsync();
            var series = await _dbSet.OfType<Series>().ToListAsync();

            return movies.Cast<Media>().Concat(series.Cast<Media>()).ToList();
        }

        public async Task<ICollection<Series>> GetAllSeriesAsync()
        {
            return await _dbSet
                .OfType<Series>()
                .ToListAsync();
        }

        public async Task<ICollection<Movie>> GetAllMoviesAsync()
        {
            return await _dbSet
                .OfType<Movie>()
                .ToListAsync();
        }

        public async Task<Movie> CreateMovieAsync(Movie movie)
        {
            await _dbSet.AddAsync(movie);
            await SaveChangesAsync();
            return movie;
        }

        public async Task<Series> CreateSeriesAsync(Series series)
        {
            await _dbSet.AddAsync(series);
            await SaveChangesAsync();
            return series;
        }

        public async Task<Episode> CreateEpisodeAsync(Episode episode)
        {
            await _dbSet.AddAsync(episode);
            await SaveChangesAsync();
            return episode;
        }

        public async Task<int> GetLastEpisodeOfASeasonAsync(Guid seriesId, int seasonNumber)
        {
            var lastEpisodeNumer =  await _dbSet
                .OfType<Episode>()
                .Where(e => e.SeriesId == seriesId && e.SeasonNumber == seasonNumber)
                .MaxAsync(e => e.EpisodeNumber);

            if (lastEpisodeNumer != null) 
            {
                return lastEpisodeNumer.Value;
            }
            else
            {
                return 0;
            }
        }

        public async Task<Movie?> GetMovieByIdAsync(Guid id)
        {
            return await _dbSet 
                .OfType<Movie>()
                .Where(m  => m.Id == id)
                .Include(m => m.Reviews!)
                .ThenInclude(r => r.UserProfile)
                .FirstOrDefaultAsync();
        }

        public async Task<Series?> GetSeriesByIdAsync(Guid id)
        {
            return await _dbSet
                .OfType<Series>()
                .Where(s => s.Id == id)
                .Include(s => s.Episodes)
                .Include(s => s.Reviews!)
                .ThenInclude(r => r.UserProfile)
                .FirstOrDefaultAsync();
        }

        public async Task<Episode?> GetEpisodeByIdAsync(Guid id)
        {
            return await _dbSet
                .OfType<Episode>()
                .Where(e => e.Id == id)
                .Include(e => e.Series)
                .Include(e => e.Reviews!)
                .ThenInclude(r => r.UserProfile)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateMovie(Movie movie)
        {
            _dbSet.Update(movie);
            await SaveChangesAsync();
        }

        public async Task UpdateSeries(Series series)
        {
            _dbSet.Update(series);
            await SaveChangesAsync();
        }

        public async Task UpdateEpisode(Episode episode)
        {
            _dbSet.Update(episode);
            await SaveChangesAsync();
        }
    }
}

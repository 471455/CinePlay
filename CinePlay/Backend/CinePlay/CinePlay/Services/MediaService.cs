using MyMDb.RepositoryInterfaces;
using MyMDb.Models;
using MyMDb.ServiceInterfaces;
using System.Diagnostics;

namespace MyMDb.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _MediaRepository;
        private readonly IConfiguration _configuration;

        public MediaService(IMediaRepository MediaRepository, IConfiguration configuration)
        {
            _MediaRepository = MediaRepository;
            _configuration = configuration;
        }

        // getting
        public async Task<ICollection<Media>> GetAllMedia()
        {
            return await _MediaRepository.GetAllMediaAsync();
        }

        public async Task<ICollection<Movie>> GetAllMovies()
        {
            return await _MediaRepository.GetAllMoviesAsync();
        }

        public async Task<ICollection<Series>> GetAllSeries()
        {
            return await _MediaRepository.GetAllSeriesAsync();
        }

        public async Task<ICollection<Episode>> GetEpisodesOfASeries(Guid seriesId)
        {
            return await _MediaRepository.GetEpisodesBySeriesIdAsync(seriesId);
        }

        public async Task<Media?> GetById(Guid id)
        {
            return await _MediaRepository.GetByIdAsync(id);
        }

        // adding
        public async Task<Movie> AddMovie(Movie movie)
        {
            var newMovie = movie;

            newMovie.Initialize();

            return await _MediaRepository.CreateMovieAsync(newMovie);
        }

        public async Task<Series> AddSeries(Series series)
        {
            var newSeries = series;

            newSeries.Initialize();

            return await _MediaRepository.CreateSeriesAsync(newSeries);
        }

        public async Task<Episode> AddEpisode(Episode episode)
        {
            var newEpisode = episode;

            newEpisode.Initialize();

            if (newEpisode.EpisodeNumber == null)
            {
                newEpisode.EpisodeNumber = await _MediaRepository.GetLastEpisodeOfASeasonAsync(episode.SeriesId, episode.SeasonNumber) + 1;
            }

            return await _MediaRepository.CreateEpisodeAsync(newEpisode);
        }

        public async Task<ICollection<Episode>> AddManyEpisodesToASeries(Guid seriesId, int seasonNumber, int episodesNumber, string? posterPath)
        {
            var lastEpisodeNumber = await _MediaRepository.GetLastEpisodeOfASeasonAsync(seriesId, seasonNumber);
            var newEpisodes = new List<Episode>();

            for (var episodeNumber = 1; episodeNumber <= episodesNumber; episodeNumber++)
            {
                var newEpisode = new Episode();
                newEpisode.SeriesId = seriesId;
                newEpisode.SeasonNumber = seasonNumber;
                newEpisode.EpisodeNumber = lastEpisodeNumber + episodeNumber;
                newEpisode.PosterPath = posterPath;

                newEpisode.Initialize();

                newEpisode = await _MediaRepository.CreateEpisodeAsync(newEpisode);

                newEpisodes.Add(newEpisode);
            }

            return newEpisodes;
        }

        // editing

        public async Task<Movie?> EditMovie(Guid id, Movie editedMovie)
        {
            var movieToEdit = await _MediaRepository.GetMovieByIdAsync(id);

            if (movieToEdit == null)
            {
                return null;
            }

            movieToEdit = editedMovie;

            movieToEdit.UpdateDateModified();

            await _MediaRepository.UpdateMovie(movieToEdit);

            return movieToEdit;
        }

        public async Task<Series?> EditSeries(Guid id, Series editedSeries)
        {
            var seriesToEdit = await _MediaRepository.GetSeriesByIdAsync(id);

            if (seriesToEdit == null)
            {
                return null;
            }

            seriesToEdit = editedSeries;

            seriesToEdit.UpdateDateModified();

            await _MediaRepository.UpdateSeries(seriesToEdit);

            return seriesToEdit;
        }

        public async Task<Episode?> EditEpisode(Guid id, Episode editedEpisode)
        {
            var episodeToEdit = await _MediaRepository.GetEpisodeByIdAsync(id);

            if (episodeToEdit == null)
            {
                return null;
            }

            episodeToEdit = editedEpisode;

            episodeToEdit.UpdateDateModified();

            await _MediaRepository.UpdateEpisode(episodeToEdit);

            return episodeToEdit;
        }

        // removing

        public async Task<bool> DeleteMedia(Guid id)
        {
            var mediaToDelete = await _MediaRepository.GetByIdAsync(id);

            if (mediaToDelete == null)
            {
                return false;
            }

            // sterge poster si video odata cu movie
            if (mediaToDelete.PosterPath != null && System.IO.File.Exists(Path.Combine(_configuration["Paths:Root"]!, mediaToDelete.PosterPath)))
            {
                System.IO.File.Delete(Path.Combine(_configuration["Paths:Root"]!, mediaToDelete.PosterPath));
            }

            if (mediaToDelete.VideoPath != null && System.IO.File.Exists(Path.Combine(_configuration["Paths:Root"]!, mediaToDelete.VideoPath)))
            {
                System.IO.File.Delete(Path.Combine(_configuration["Paths:Root"]!, mediaToDelete.VideoPath));
            }

            await _MediaRepository.Delete(mediaToDelete);

            return true;
        }

        // getting by id

        public async Task<Movie?> GetMovieById(Guid id)
        {
            return await _MediaRepository.GetMovieByIdAsync(id);
        }

        public async Task<Series?> GetSeriesById(Guid id)
        {
            return await _MediaRepository.GetSeriesByIdAsync(id);
        }

        public async Task<Episode?> GetEpisodeById(Guid id)
        {
            return await _MediaRepository.GetEpisodeByIdAsync(id);
        }


        public async Task NormalizeVideo(string videoPath)
        {
            string scriptPath;
            string shell;

            videoPath = Path.Combine(Directory.GetCurrentDirectory(), videoPath);

            // Sadece Windows desteği
            scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "shell_scripts\\convert_to_mp4_aac.bat");
            shell = "cmd.exe";

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = shell,
                Arguments = $"/c \"{scriptPath} \"{videoPath}\"\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(processStartInfo)!)
                {
                    var outputTask = process.StandardOutput.ReadToEndAsync();
                    var errorsTask = process.StandardError.ReadToEndAsync();

                    await process.WaitForExitAsync();

                    var output = await outputTask;
                    var errors = await errorsTask;

                    Console.WriteLine("Output:");
                    Console.WriteLine(output);
                    Console.WriteLine("Errors:");
                    Console.WriteLine(errors);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("sf");
        }



        //public async Task NormalizeVideo(string videoPath)
        //{
        //    string scriptPath;
        //    string shell;

        //    videoPath = Path.Combine(Directory.GetCurrentDirectory(), videoPath);

        //    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        //    {
        //        // Windows
        //        scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "shell_scripts\\convert_to_mp4_aac.bat");
        //        shell = "cmd.exe";
        //    }
        //    else
        //    {
        //        // Linux
        //        scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "shell_scripts\\convert_to_mp4_aac.sh"); ;
        //        shell = "/bin/bash";
        //    }

        //    ProcessStartInfo processStartInfo = new ProcessStartInfo
        //    {
        //        FileName = shell,
        //        Arguments = (Environment.OSVersion.Platform == PlatformID.Win32NT)
        //                    ? $"/c \"{scriptPath} \"{videoPath}\"\""
        //                    : $"-c \"{scriptPath} \"{videoPath}\"\"",
        //        RedirectStandardOutput = true,
        //        RedirectStandardError = true,
        //        UseShellExecute = false,
        //        CreateNoWindow = true
        //    };

        //    try
        //    {
        //        using (Process process = Process.Start(processStartInfo)!)
        //        {
        //            var outputTask = process.StandardOutput.ReadToEndAsync();
        //            var errorsTask = process.StandardError.ReadToEndAsync();

        //            await process.WaitForExitAsync();

        //            var output = await outputTask;
        //            var errors = await errorsTask;

        //            Console.WriteLine("Output:");
        //            Console.WriteLine(output);
        //            Console.WriteLine("Errors:");
        //            Console.WriteLine(errors);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //    }

        //    Console.WriteLine("sf");
        //}
    }
}

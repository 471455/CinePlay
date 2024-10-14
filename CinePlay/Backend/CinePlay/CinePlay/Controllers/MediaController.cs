using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyMDb.DTOs;
using MyMDb.Models;
using MyMDb.ServiceInterfaces;
using MyMDb.Data;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace MyMDb.Controllers
{
    [ApiController]
    [Route("api/media")]
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public MediaController(IMediaService mediaService, IMapper mapper, IConfiguration configuration, ApplicationDbContext context)
        {
            _mediaService = mediaService;
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        [Route("test")]
        public IActionResult TestConnection()
        {
            try
            {
                _context.Database.GetDbConnection().Open();
                return Ok("Database connection is successful.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database connection failed: {ex.Message}");
            }
            finally
            {
                _context.Database.GetDbConnection().Close();
            }
        }

        // -------------------- get all

        [HttpGet]
        [Route("movies_and_series")]
        public async Task<IActionResult> GetMoviesAndSeries()
        {
            var allMedia = await _mediaService.GetAllMedia();
            var mediaDtos = _mapper.Map<List<MediaDto>>(allMedia);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(mediaDtos);
        }

        [HttpGet]
        [Route("movies")]
        public async Task<IActionResult> GetMovies()
        {
            var allMovies = await _mediaService.GetAllMovies();
            var moviesDtos = _mapper.Map<List<MovieDto>>(allMovies);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(moviesDtos);
        }

        [HttpGet]
        [Route("series")]
        public async Task<IActionResult> GetSeries()
        {
            var allSeries = await _mediaService.GetAllSeries();
            var seriesDtos = _mapper.Map<List<SeriesDto>>(allSeries);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(seriesDtos);
        }

        // -------------------- get by id

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMedia(Guid id)
        {
            var media = await _mediaService.GetById(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (media == null)
            {
                return NotFound();
            }

            if (media.MediaType == "Movie")
            {
                var movie = await _mediaService.GetMovieById(id);
                return Ok(movie);
            }
            else if (media.MediaType == "Series")
            {
                var series = await _mediaService.GetSeriesById(id);
                return Ok(series);
            }
            else if (media.MediaType == "Episode")
            {
                var episode = await _mediaService.GetEpisodeById(id);
                return Ok(episode);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("series/{seriesId}/episodes")]
        public async Task<IActionResult> GetEpisodesOfASeries(Guid seriesId)
        {
            var episodes = await _mediaService.GetEpisodesOfASeries(seriesId);
            var episodeDtos = _mapper.Map<List<EpisodeDto>>(episodes);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (episodeDtos.Count == 0)
            {
                return NotFound();
            }

            return Ok(episodeDtos);
        }

        // -------------------- add

        [HttpPost]
        [Authorize("admin")]
        [Route("add_movie")]
        public async Task<IActionResult> AddMovie([FromForm] MovieDto movie, IFormFile? poster, IFormFile? video)
        {
            var newMovie = _mapper.Map<Movie>(movie);

            if (poster != null)
            {
                if (!Extensions.IsImageFile(poster.FileName))
                {
                    return BadRequest("Not an image file provided for poster.");
                }
                if (newMovie.PosterPath == null)
                {
                    return BadRequest("No path provided for poster.");
                }

                newMovie.PosterPath = _configuration["Paths:Images"] + newMovie.PosterPath;

                using (var stream = new FileStream(_configuration["Paths:Root"] + newMovie.PosterPath, FileMode.Create))
                {
                    await poster.CopyToAsync(stream);
                }
            }

            if (video != null)
            { 

                if (!Extensions.IsVideoFile(video.FileName))
                {
                    return BadRequest("Not a video file provided for video.");
                }
                if (newMovie.VideoPath == null)
                {
                    return BadRequest("No path provided for video.");
                }

                newMovie.VideoPath = _configuration["Paths:Videos"] + newMovie.VideoPath;

                using (var stream = new FileStream(_configuration["Paths:Root"] + newMovie.VideoPath, FileMode.Create))
                {
                    await video.CopyToAsync(stream);
                }

                // Process the vide to make it browser accepted without blocking execution
                _mediaService.NormalizeVideo(_configuration["Paths:Root"]! + newMovie.VideoPath);
            }

            newMovie = await _mediaService.AddMovie(newMovie);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(newMovie);
        }

        [HttpPost]
        [Authorize("admin")]
        [Route("add_series")]
        public async Task<IActionResult> AddSeries([FromForm] SeriesDto series, IFormFile? poster)
        {
            var newSeries = _mapper.Map<Series>(series);

            // Setting up series directory
            var seriesImagesDirectory = Path.Combine(_configuration["Paths:Root"]!, _configuration["Paths:Images"]!, newSeries.Title);
            if (!Directory.Exists(seriesImagesDirectory))
            {
                Directory.CreateDirectory(seriesImagesDirectory);
            }

            var seriesVideosDirectory = Path.Combine(_configuration["Paths:Root"]!, _configuration["Paths:Videos"]!, newSeries.Title);
            if (!Directory.Exists(seriesVideosDirectory))
            {
                Directory.CreateDirectory(seriesVideosDirectory);
            }

            if (poster != null)
            {
                if (!Extensions.IsImageFile(poster.FileName))
                {
                    return BadRequest("Not an image file provided for poster.");
                }
                if (newSeries.PosterPath == null)
                {
                    return BadRequest("No path provided for poster");
                }

                newSeries.PosterPath = Path.Combine(_configuration["Paths:Images"]!, newSeries.Title, newSeries.PosterPath);

                using (var stream = new FileStream(_configuration["Paths:Root"] + newSeries.PosterPath, FileMode.Create))
                {
                    await poster.CopyToAsync(stream);
                }
            }

            newSeries = await _mediaService.AddSeries(newSeries);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(newSeries);
        }

        [HttpPost]
        [Authorize("admin")]
        [Route("add_episode")]
        public async Task<IActionResult> AddEpisode([FromForm] EpisodeDto episode, IFormFile? poster, IFormFile? video)
        {
            var newEpisode = _mapper.Map<Episode>(episode);

            var series = await _mediaService.GetSeriesById(newEpisode.SeriesId);

            if (series == null) 
            {
                return BadRequest("The episode is added to an non-existent series");
            }

            if (poster != null)
            {
                if (!Extensions.IsImageFile(poster.FileName))
                {
                    return BadRequest("Not an image file provided for poster.");
                }
                if (newEpisode.PosterPath == null)
                {
                    return BadRequest("No path provided for poster");
                }

                newEpisode.PosterPath = Path.Combine(_configuration["Paths:Images"]!, series.Title, newEpisode.PosterPath);

                using (var stream = new FileStream(_configuration["Paths:Root"] + newEpisode.PosterPath, FileMode.Create))
                {
                    await poster.CopyToAsync(stream);
                }
            }

            if (video != null)
            {
                if (!Extensions.IsVideoFile(video.FileName))
                {
                    return BadRequest("Not a video file provided for video.");
                }
                if (newEpisode.VideoPath == null)
                {
                    return BadRequest("No path provided for video");
                }

                newEpisode.VideoPath = Path.Combine(_configuration["Paths:Videos"]!, series.Title, newEpisode.VideoPath);

                using (var stream = new FileStream(_configuration["Paths:Root"] + newEpisode.VideoPath, FileMode.Create))
                {
                    await video.CopyToAsync(stream);
                }
            }

            newEpisode = await _mediaService.AddEpisode(newEpisode);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(newEpisode);
        }

        [HttpPost]
        [Authorize("admin")]
        [Route("add_many_episodes")]
        public async Task<IActionResult> AddManyEpisodes(Guid seriesId, int seasonNumber, int episodesNumber)
        {
            var series = await _mediaService.GetSeriesById(seriesId);

            if (series == null)
            {
                return NotFound("Series not found");
            }

            var newEpisodes = await _mediaService.AddManyEpisodesToASeries(seriesId, seasonNumber, episodesNumber, series.PosterPath);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(newEpisodes);
        }

        // -------------------- edit

        [HttpPost]
        [Authorize("admin")]
        [Route("edit_movie/{id}")]
        public async Task<IActionResult> EditMovie(Guid id, [FromForm] MovieDto movieToEdit, IFormFile? poster, IFormFile? video)
        {
            if (id != movieToEdit.Id)
            {
                return BadRequest();
            }

            var currentMovie = await _mediaService.GetMovieById(id);
            if (currentMovie == null)
            {
                return NotFound();
            }

            if (poster != null)
            {
                if (!Extensions.IsImageFile(poster.FileName))
                {
                    return BadRequest("Not an image file provided for poster.");
                }
                if (movieToEdit.PosterPath == null)
                {
                    return BadRequest("No path provided for poster.");
                }

                movieToEdit.PosterPath = _configuration["Paths:Images"] + movieToEdit.PosterPath;

                using (var stream = new FileStream(_configuration["Paths:Root"] + movieToEdit.PosterPath, FileMode.Create))
                {
                    await poster.CopyToAsync(stream);
                }
            }

            if (video != null)
            {
                if (!Extensions.IsVideoFile(video.FileName))
                {
                    return BadRequest("Not a video file provided for video.");
                }
                if (movieToEdit.VideoPath == null)
                {
                    return BadRequest("No path provided for video.");
                }

                movieToEdit.VideoPath = _configuration["Paths:Videos"] + movieToEdit.VideoPath;

                using (var stream = new FileStream(_configuration["Paths:Root"] + movieToEdit.VideoPath, FileMode.Create))
                {
                    await video.CopyToAsync(stream);
                }
            }

            _mapper.Map(movieToEdit, currentMovie);

            await _mediaService.EditMovie(id, currentMovie);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(currentMovie);
        }

        [HttpPost]
        [Authorize("admin")]
        [Route("edit_series/{id}")]
        public async Task<IActionResult> EditSeries(Guid id, [FromBody] SeriesDto seriesToEdit, IFormFile? poster)
        {
            if (id != seriesToEdit.Id)
            {
                return BadRequest();
            }

            var currentSeries = await _mediaService.GetSeriesById(id);
            if (currentSeries == null)
            {
                return NotFound();
            }

            if (poster != null)
            {
                if (seriesToEdit.PosterPath == null)
                {
                    return BadRequest("No path provided for poster");
                }
                using (var stream = new FileStream(seriesToEdit.PosterPath, FileMode.Create))
                {
                    await poster.CopyToAsync(stream);
                }
                currentSeries.PosterPath = seriesToEdit.PosterPath;
            }

            _mapper.Map(seriesToEdit, currentSeries);

            await _mediaService.EditSeries(id, currentSeries);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(currentSeries);
        }

        [HttpPost]
        [Authorize("admin")]
        [Route("edit_episode/{id}")]
        public async Task<IActionResult> EditEpisode(Guid id, [FromBody] EpisodeDto episodeToEdit, IFormFile? poster, IFormFile? video)
        {
            if (id != episodeToEdit.Id)
            {
                return BadRequest();
            }

            var currentEpisode = await _mediaService.GetEpisodeById(id);
            if (currentEpisode == null)
            {
                return NotFound();
            }

            if (poster != null)
            {
                if (episodeToEdit.PosterPath == null)
                {
                    return BadRequest("No path provided for poster");
                }
                using (var stream = new FileStream(episodeToEdit.PosterPath, FileMode.Create))
                {
                    await poster.CopyToAsync(stream);
                }
                currentEpisode.PosterPath = episodeToEdit.PosterPath;
            }

            if (video != null)
            {
                if (episodeToEdit.VideoPath == null)
                {
                    return BadRequest("No path provided for video");
                }
                using (var stream = new FileStream(episodeToEdit.VideoPath, FileMode.Create))
                {
                    await video.CopyToAsync(stream);
                }
                currentEpisode.VideoPath = episodeToEdit.VideoPath;
            }

            _mapper.Map(episodeToEdit, currentEpisode);

            await _mediaService.EditEpisode(id, currentEpisode);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(currentEpisode);
        }

        // -------------------- delete

        [HttpDelete]
        [Authorize("admin")]
        [Route("delete_media/{id}")]
        public async Task<IActionResult> DeleteMedia(Guid id)
        {
            var deleted = await _mediaService.DeleteMedia(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}

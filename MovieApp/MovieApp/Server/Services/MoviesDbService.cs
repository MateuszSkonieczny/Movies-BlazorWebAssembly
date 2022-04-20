using Microsoft.EntityFrameworkCore;
using MovieApp.Server.Data;
using MovieApp.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Shared.DTOs;

namespace MovieApp.Server.Services
{
    public interface IMoviesRepository
    {

    }

    public interface IMoviesDbService
    {
        Task<List<Movie>> GetMovies();
        Task AddMovie(MovieDto movieDto);
        Task<MovieDto> GetMovie(int movieId);
        Task UpdateMovie(MovieDto movieDto, int id);
        Task<bool> DeleteMovie(int id);
    }

    public class MoviesDbService : IMoviesDbService
    {
        private ApplicationDbContext _context;

        public MoviesDbService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMovie(MovieDto movieDto)
        {
            await _context.AddAsync(new Movie
            {
                Title = movieDto.Title,
                Summary = movieDto.Summary,
                InTheaters = movieDto.InTheaters,
                Trailer = movieDto.Trailer,
                ReleaseDate = movieDto.ReleaseDate,
                Poster = movieDto.Poster
            });
            await _context.SaveChangesAsync();
        }

        public async Task<MovieDto> GetMovie(int movieId)
        {
            var movie =  await _context.Movies.FirstOrDefaultAsync(e => e.Id == movieId);
            var res = new MovieDto
            {
                Title = movie.Title,
                Summary = movie.Summary,
                InTheaters = movie.InTheaters,
                Trailer = movie.Trailer,
                ReleaseDate = movie.ReleaseDate,
                Poster = movie.Poster
            };
            
            return res;

        }

        public async Task UpdateMovie(MovieDto movieDto, int id)
        {
            var res = await _context.Movies.SingleAsync(e => e.Id == id);

            res.Title = movieDto.Title;
            res.Summary = movieDto.Summary;
            res.InTheaters = movieDto.InTheaters;
            res.Trailer = movieDto.Trailer;
            res.ReleaseDate = movieDto.ReleaseDate;
            res.Poster = movieDto.Poster;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteMovie(int id)
        {
            var boolek = await _context.MoviesActors.AnyAsync(e => e.MovieId == id);

            if (!boolek)
            {
                boolek = await _context.MoviesGenres.AnyAsync(e => e.MovieId == id);
            }

            if (boolek)
            {
                return false;
            }

            var movie = await _context.Movies.SingleAsync(e => e.Id == id);
            _context.Remove(movie);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Movie>> GetMovies()
        {
            return await _context.Movies.OrderBy(m => m.Title).ToListAsync();
        }
    }
}

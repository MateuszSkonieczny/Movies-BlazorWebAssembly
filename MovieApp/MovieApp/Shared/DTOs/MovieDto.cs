using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApp.Shared.DTOs
{
    public class MovieDto
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        
        [Required(ErrorMessage = "In theaters is required")]
        public bool InTheaters { get; set; }
        public string Trailer { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Poster { get; set; }
    }
}
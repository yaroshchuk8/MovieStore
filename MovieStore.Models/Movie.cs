using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MovieStore.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        [Range(1, 1000)]
        public double Price { get; set; }
        
        [ValidateNever]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [ValidateNever]
        public List<Genre> Genres { get; set; }
    }
}

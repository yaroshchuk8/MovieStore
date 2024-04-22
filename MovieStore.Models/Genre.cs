using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MovieStore.Models
{
	public class Genre
	{
		[Key]
		public int Id { get; set; }
		
		[Required]
		public string Name { get; set; }

		[ValidateNever]
		public List<Movie> Movies { get; set; }
	}
}

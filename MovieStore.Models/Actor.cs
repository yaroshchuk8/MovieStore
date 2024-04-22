using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Models
{
	public class Actor
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[ValidateNever]
		[Display(Name = "Image")]
		public string ImageUrl { get; set; }

		[ValidateNever]
		public List<Movie> Movies { get; set; }
	}
}

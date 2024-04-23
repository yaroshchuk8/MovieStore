using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MovieStore.Models.ViewModels
{
	public class MovieVM
	{
		public Movie Movie { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem> GenreList { get; set; }

		[ValidateNever]
		public List<string> SelectedGenres { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ActorList { get; set; }

        [ValidateNever]
        public List<string> SelectedActors { get; set; }
    }
}

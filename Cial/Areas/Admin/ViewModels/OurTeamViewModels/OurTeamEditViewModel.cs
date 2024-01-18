using Cial.Models;
using System.ComponentModel.DataAnnotations;

namespace Cial.Areas.Admin.ViewModels.OurTeamViewModels
{
    public class OurTeamEditViewModel
    {
        [Required, MaxLength(32)]
        public string JobTitle { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}

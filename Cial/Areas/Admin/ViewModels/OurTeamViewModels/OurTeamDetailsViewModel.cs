using Cial.Models;

namespace Cial.Areas.Admin.ViewModels.OurTeamViewModels
{
    public class OurTeamDetailsViewModel
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string Image { get; set; }
        public bool IsDeleted { get; set; }
    }
}

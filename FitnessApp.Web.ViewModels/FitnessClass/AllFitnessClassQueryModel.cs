using FitnessApp.Web.Infrastructure.Enumerations;

namespace FitnessApp.Web.ViewModels.FitnessClass
{
    public class AllFitnessClassQueryModel
    {
        public int FitnessClassesPerPage { get; } = 5;

        public string Category { get; init; } = null!;

        public string SearchTerm { get; set; } = null!;

        public FitnessClassSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalFitnessClassesCount { get; set; }

        public IEnumerable<string> Categories { get; set; } = null!;

        public IEnumerable<FitnessClassServiceModel> FitnessClasses { get; set; } = new List<FitnessClassServiceModel>();

        public List<string> Statuses { get; set; } = new(){ "Active", "Canceled", "Finished" };

        public string Status { get; init; } = null!;
    }
}

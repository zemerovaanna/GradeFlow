using System.Collections.ObjectModel;
using System.Windows.Input;
using GradeFlowECTS.Core;
using GradeFlowECTS.Models;
using GradeFlowECTS.ViewModel.Items;

namespace GradeFlowECTS.ViewModel
{
    public class AddCriterionViewModel : BaseViewModel
    {
        private byte count;
        public ICommand AddScoreCommand { get; set; }

        public AddCriterionViewModel(IEnumerable<Module> modules)
        {
            AllModules = new ObservableCollection<Module>(modules);
            ScoreOptions = new ObservableCollection<ScoreOptionViewModel>();
            AddScoreCommand = new RelayCommand(_ => AddScoreOption());

            SelectedModule = AllModules.FirstOrDefault();
            count = 0;
        }

        public ObservableCollection<Module> AllModules { get; }

        public int CriterionNumber { get; set; }

        public string CriterionTitle { get; set; } = string.Empty;

        public Module? SelectedModule { get; set; }

        public ObservableCollection<ScoreOptionViewModel> ScoreOptions { get; }

        public void AddScoreOption()
        {
            if (count < 5)
            {
                count++;
                var scoreOption = new ScoreOption
                {
                    ScoreValue = 0,
                    Description = string.Empty
                };
                ScoreOptions.Add(new ScoreOptionViewModel(scoreOption));
            }
        }

        public Criterion ToCriterion()
        {
            return new Criterion
            {
                CriterionNumber = CriterionNumber,
                CriterionTitle = CriterionTitle,
                ModuleId = SelectedModule.ModuleId,
                Module = SelectedModule,
                ScoreOptions = ScoreOptions.Select(s => new ScoreOption
                {
                    ScoreValue = s.ScoreValue,
                    Description = s.Description
                }).ToList()
            };
        }
    }
}
using System.Collections.ObjectModel;
using GradeFlowECTS.Core;
using GradeFlowECTS.Models;

namespace GradeFlowECTS.ViewModel.Items
{
    public class CriterionViewModel : BaseViewModel
    {
        public Criterion Model { get; }

        public CriterionViewModel(Criterion criterion)
        {
            Model = criterion;

            ScoreOptions = new ObservableCollection<ScoreOptionViewModel>(
                Model.ScoreOptions.OrderBy(o => o.ScoreValue).Select(o =>
                {
                    var vm = new ScoreOptionViewModel(o);
                    vm.PropertyChanged += (_, __) => OnPropertyChanged(nameof(MaxScore));
                    return vm;
                }));

            ScoreOptions.CollectionChanged += (_, __) => OnPropertyChanged(nameof(MaxScore));
        }


        public int CriterionNumber
        {
            get => Model.CriterionNumber;
            set
            {
                if (Model.CriterionNumber != value)
                {
                    Model.CriterionNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CriterionTitle
        {
            get => Model.CriterionTitle;
            set
            {
                if (Model.CriterionTitle != value)
                {
                    Model.CriterionTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        public Module? Module
        {
            get => Model.Module;
            set
            {
                if (Model.Module != value)
                {
                    Model.Module = value;
                    Model.ModuleId = value?.ModuleId;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ScoreOptionViewModel> ScoreOptions { get; }

        public int MaxScore => ScoreOptions.Any() ? ScoreOptions.Max(x => x.ScoreValue) : 0;
    }
}
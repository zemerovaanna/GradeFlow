using System.Collections.ObjectModel;
using GradeFlowECTS.Core;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using GradeFlowECTS.ViewModel.Items;

namespace GradeFlowECTS.ViewModel
{
    public class EditCriteriaViewModel : BaseViewModel
    {
        public ObservableCollection<CriterionViewModel> Criteria { get; set; }
        public ObservableCollection<Module> Modules { get; set; }

        private readonly CriterionRepository _repository;

        public EditCriteriaViewModel()
        {
            _repository = new CriterionRepository();
            LoadData();
        }

        public void LoadData()
        {
            Criteria = new ObservableCollection<CriterionViewModel>(
                _repository.GetAllCriteriaWithDetails()
                           .Select(c => new CriterionViewModel(c)));

            Modules = new ObservableCollection<Module>(_repository.GetAllModules());

            OnPropertyChanged(nameof(Criteria));
            OnPropertyChanged(nameof(Modules));
        }

        public void SaveChanges()
        {
            foreach (var vm in Criteria)
            {
                vm.Model.ScoreOptions = vm.ScoreOptions
                    .Select(optVm => optVm.Model)
                    .ToList();
            }

            var entities = Criteria.Select(vm => vm.Model).ToList();
            _repository.SaveChanges(entities);
        }

        public void DeleteCriterion(CriterionViewModel viewModel)
        {
            Criteria.Remove(viewModel);
            _repository.DeleteCriterion(viewModel.Model);
        }
    }
}
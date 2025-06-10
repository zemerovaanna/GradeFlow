using System.DirectoryServices;
using GradeFlowECTS.Core;
using GradeFlowECTS.Models;

namespace GradeFlowECTS.ViewModel.Items
{
    public class ScoreOptionViewModel : BaseViewModel
    {
        public ScoreOption Model { get; }

        public ScoreOptionViewModel(ScoreOption option)
        {
            Model = option;
        }

        public int ScoreValue
        {
            get => Model.ScoreValue;
            set
            {
                if (Model.ScoreValue != value)
                {
                    Model.ScoreValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => Model.Description;
            set
            {
                if (Model.Description != value)
                {
                    Model.Description = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
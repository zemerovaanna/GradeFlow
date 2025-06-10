using System.ComponentModel;
using GradeFlowECTS.Core;
using GradeFlowECTS.Models;

namespace GradeFlowECTS.ViewModel.Items
{
    public class QuestionAnswerViewModel : BaseViewModel
    {
        public QuestionAnswer Model { get; }

        public QuestionAnswerViewModel(QuestionAnswer model)
        {
            Model = model;
        }

        public string AnswerText
        {
            get => Model.QuestionAnswerText;
            set { Model.QuestionAnswerText = value; OnPropertyChanged(nameof(AnswerText)); }
        }

        public bool Correct
        {
            get => Model.IsCorrect;
            set { Model.IsCorrect = value; OnPropertyChanged(nameof(Correct)); }
        }

        public byte[] FileData
        {
            get => Model.FileData;
            set { Model.FileData = value; OnPropertyChanged(nameof(FileData)); }
        }

        public string FileName
        {
            get => Model.FileName;
            set { Model.FileName = value; OnPropertyChanged(nameof(FileName)); }
        }

        public QuestionAnswer ToModel()
        {
            return new QuestionAnswer
            {
                QuestionAnswerText = this.AnswerText,
                IsCorrect = this.Correct,
                FileData = this.FileData,
                FileName = this.FileName
            };
        }
    }
}
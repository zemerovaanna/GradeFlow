using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GradeFlowECTS.Models;
using GradeFlowECTS.Repositories;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Win32;

namespace GradeFlowECTS.Analyzers
{
    public class AnalyzerViewModel : INotifyPropertyChanged
    {
        private int _totalScore;

        public int TotalScore
        {
            get { return _totalScore; }
            set
            {
                if (_totalScore != value)
                {
                    _totalScore = value;
                    OnPropertyChanged(nameof(TotalScore));
                }
            }
        }

        public ObservableCollection<string> FilePaths { get; } = new();
        public ObservableCollection<CriterionResult> CriteriaResults { get; } = new();

        public ICommand LoadFilesCommand => new RelayCommand(_ => LoadFiles());
        public ICommand AnalyzeCommand => new RelayCommand(_ => AnalyzeFiles());

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void LoadFiles()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "C# files (*.cs)|*.cs",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                FilePaths.Clear();
                foreach (var path in dialog.FileNames)
                    FilePaths.Add(path);
            }
        }

        private void AnalyzeFiles()
        {
            // Чтение и разбор файлов в деревья
            var trees = FilePaths.Select(p =>
            {
                var code = File.ReadAllText(p);
                return CSharpSyntaxTree.ParseText(code);
            }).ToList();

            // Компиляция и семантические модели
            var compilation = CSharpCompilation.Create("Temp", syntaxTrees: trees);
            var models = trees.Select(t => compilation.GetSemanticModel(t)).ToList();

            // Загрузка критериев
            var repo = new CriterionRepository();
            var criteria = repo.GetAllCriteriaWithDetails();

            // Сопоставление критериев с анализаторами
            var analyzerPairs = new List<(Criterion criterion, ICriterionAnalyzer analyzer)>();
            foreach (var c in criteria)
            {
                ICriterionAnalyzer? analyzer = c.CriterionNumber switch
                {
                    1 => new Criterion1Adapter(c.MaxScore),
                    2 => new Criterion2Adapter(c.MaxScore),
                    3 => new Criterion3Adapter(c.MaxScore),
                    4 => new Criterion4Adapter(c.MaxScore),
                    5 => new Criterion5Adapter(c.MaxScore),
                    6 => new Criterion6Adapter(c.MaxScore),
                    7 => new Criterion7Adapter(c.MaxScore),
                    _ => null
                };

                if (analyzer != null)
                    analyzerPairs.Add((c, analyzer));
            }

            // Оценка и сохранение результатов
            CriteriaResults.Clear();
            TotalScore = 0;

            foreach (var (criterion, analyzer) in analyzerPairs)
            {
                var score = analyzer.Evaluate(trees, models, compilation);
                CriteriaResults.Add(new CriterionResult
                {
                    CriterionNumber = criterion.CriterionNumber,
                    CriterionTitle = criterion.CriterionTitle,
                    Score = score,
                    MaxScore = criterion.MaxScore
                });
                TotalScore += score;
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;

        public RelayCommand(Action<object?> execute)
        {
            _execute = execute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add{ }
            remove { }
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => _execute(parameter);
    }
}
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GradeFlowECTS.Analyzers.Qual
{
    public class Criterion4Analyzer
    {
        private readonly int _maxScore;

        public Criterion4Analyzer(int maxScore)
        {
            _maxScore = maxScore;
        }

        public int Evaluate(List<SyntaxTree> trees, List<SemanticModel> models)
        {
            int totalClassCount = 0;
            int singleClassFilesCount = 0;

            foreach (var tree in trees)
            {
                var root = tree.GetRoot();
                var classesInFile = root.DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .ToList();

                if (classesInFile.Any())
                {
                    totalClassCount += classesInFile.Count;

                    if (classesInFile.Count == 1)
                        singleClassFilesCount++;
                }
            }

            if (singleClassFilesCount == 0)
                return 0;
            if (singleClassFilesCount == totalClassCount)
                return _maxScore;
            return _maxScore / 2;
        }
    }
}
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using GradeFlowECTS.Data;

namespace GradeFlowECTS.Converters
{
    public class AnswerSelectionMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 3)
                return false;

            if (!(values[0] is int answerId))
                return false;

            if (!(values[1] is ObservableCollection<UserAnswer> userAnswers))
                return false;

            if (!(values[2] is int questionId))
                return false;

            var userAnswer = userAnswers.FirstOrDefault(u => u.QuestionId == questionId);
            return userAnswer != null && userAnswer.SelectedAnswerIds.Contains(answerId);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            return new object[] { Binding.DoNothing, Binding.DoNothing, Binding.DoNothing };
        }
    }
}
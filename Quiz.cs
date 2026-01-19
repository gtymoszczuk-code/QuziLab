using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuziLab
{
    class Quiz
    {
        public string Title { get; set; }
        public int QuestionsCount { get; set; }

        // Możesz tu dodać więcej pól, które przydadzą się na innych stronach
        public string Author { get; set; }
        public string BestScore { get; set; } 
    }

    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value != null; // Jeśli obiekt nie jest nullem, zwróć true

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
    }
}

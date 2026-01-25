using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace QuziLab
{
    public class Quiz
    {
        public string Title { get; set; }
        public int QuestionsCount { get; set; }
        public string Author { get; set; }
        public string BestScore { get; set; }
        public List<Question> Questions { get; set; } = new();
        public string FilePath { get; set; }
    }

  
    public class Question : INotifyPropertyChanged
    {
        private string _content;
        private List<Answer> _answers = new();

        public string Content
        {
            get => _content;
            set { _content = value; OnPropertyChanged(nameof(Content)); }
        }

        public List<Answer> Answers
        {
            get => _answers;
            set { _answers = value; OnPropertyChanged(nameof(Answers)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class Answer
    {
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class WynikModel
    {
        public string NazwaQuizu { get; set; }
        public string DataWyniku { get; set; }
        public int PunktyZdobyte { get; set; }
        public int PunktyMax { get; set; }

        public string PunktyDisplay => $"{PunktyZdobyte}/{PunktyMax}";
    }



    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value != null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    // klasa pomocnicza do wyświetlania messageboxa
    public static class Alert
    {
        public static void Show(string message)
        {
            if (Application.Current.MainWindow is MainWindow main)
            {
                main.ShowCustomMessage(message);
            }
        }
    }


}

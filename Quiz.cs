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

    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value != null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

   
}

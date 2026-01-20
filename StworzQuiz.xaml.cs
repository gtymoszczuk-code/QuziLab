using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static QuziLab.Quiz;

namespace QuziLab
{
    public partial class StworzQuiz : UserControl
    {
        public StworzQuiz()
        {
            InitializeComponent();
            DataContext = this; // binding dla ListBox
        }

        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow.contentControl.Content = new Quizy();
        }

        private void DodajPytanie_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow.contentControl.Content = new DodajPytanie(this);
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            var selected = QuestionsListBox.SelectedItems.Cast<Question>().ToList();
            foreach (var q in selected)
                Questions.Remove(q);
        }

        private void EditBT_Click(object sender, RoutedEventArgs e)
        {
            if (QuestionsListBox.SelectedItem is Question selectedQuestion)
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow.contentControl.Content = new DodajPytanie(this, selectedQuestion);
            }
            else
            {
                MessageBox.Show("Wybierz pytanie do edycji!");
            }
        }

       
    }
}

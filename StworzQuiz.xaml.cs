using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static QuziLab.Quiz;
using System.IO;
using System.Text.Json;


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

            mainWindow.contentControl.Content = new DodajPytanie(this, Questions);
        }


        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            var selected = QuestionsListBox.SelectedItems.Cast<Question>().ToList();
            foreach (var q in selected)
                Questions.Remove(q);
        }

        private void EditBT_Click(object sender, RoutedEventArgs e)
        {
            if (QuestionsListBox.SelectedItem is not Question selectedQuestion)
            {
                Alert.Show("Wybierz pytanie do edycji!");
                return;
            }

            var mainWindow = Window.GetWindow(this) as MainWindow;

            mainWindow.contentControl.Content = new DodajPytanie(this, Questions, selectedQuestion);
        }


        private void SaveQuiz_Click(object sender, RoutedEventArgs e)
        {
            string quizName = QuizTitleInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(quizName))
            {
                Alert.Show("Podaj nazwę quizu!");
                return;
            }

            if (Questions.Count == 0)
            {
                Alert.Show("Quiz musi zawierać przynajmniej jedno pytanie!");
                return;
            }

            // Tworzymy folder Quizy jeśli nie istnieje
            string projectFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string folder = Path.Combine(projectFolder, "Quizy");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // Tworzymy obiekt quizu
            Quiz quiz = new Quiz
            {
                Title = quizName,
                QuestionsCount = Questions.Count,
                Questions = Questions.ToList()

            };

            string filePath = Path.Combine(folder, quizName + ".json");

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(quiz, options);
            File.WriteAllText(filePath, json);

            Alert.Show($"Quiz zapisany do pliku {filePath}");

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
                mainWindow.contentControl.Content = new Quizy();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static QuziLab.Quiz;

namespace QuziLab
{
   
    public partial class EdytujQuiz : UserControl
    {
        private readonly Quiz _quiz;
        public ObservableCollection<Question> Questions { get; set; }
        public EdytujQuiz(Quiz quiz)
        {
            InitializeComponent();
            _quiz = quiz;
            LoadQuiz();
        }
        private void LoadQuiz()
        {
            Questions = new ObservableCollection<Question>(_quiz.Questions);
            QuestionsListBox.ItemsSource = Questions;

            // tytuł quizu
            QuizTitleInput.Text = _quiz.Title;
        }

        

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow.contentControl.Content = new Quizy();
        }

        private void DodajPytanie_Click(object sender, RoutedEventArgs e)
        {
            if (_quiz == null) return;

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.contentControl.Content = new DodajPytanie(this, Questions);
            }
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
            string newQuizName = QuizTitleInput.Text?.Trim();

            if (string.IsNullOrWhiteSpace(newQuizName))
            {
                Alert.Show("Podaj nazwę quizu!");
                return;
            }

            if (Questions.Count == 0)
            {
                Alert.Show("Quiz musi zawierać przynajmniej jedno pytanie!");
                return;
            }

            // 📌 zapamiętujemy STARĄ nazwę (przed edycją)
            string oldQuizName = _quiz.Title;

            string projectFolder = Directory
                .GetParent(AppDomain.CurrentDomain.BaseDirectory)
                .Parent.Parent.FullName;

            string folder = System.IO.Path.Combine(projectFolder, "Quizy");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string oldFilePath = System.IO.Path.Combine(folder, oldQuizName + ".json");
            string newFilePath = System.IO.Path.Combine(folder, newQuizName + ".json");

            try
            {
                // 1️⃣ usuwamy STARY plik (jeśli istnieje)
                if (File.Exists(oldFilePath))
                    File.Delete(oldFilePath);

                // 2️⃣ tworzymy NOWY obiekt quizu
                Quiz newQuiz = new Quiz
                {
                    Title = newQuizName,
                    Questions = Questions.ToList(),
                    QuestionsCount = Questions.Count
                };

                // 3️⃣ zapisujemy NOWY plik
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(newQuiz, options);
                File.WriteAllText(newFilePath, json);

                Alert.Show("Quiz zapisany!");

                // 4️⃣ wracamy do listy quizów
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                    mainWindow.contentControl.Content = new Quizy();
            }
            catch (UnauthorizedAccessException)
            {
                Alert.Show("Brak uprawnień do zapisu pliku.");
            }
            catch (Exception ex)
            {
                Alert.Show($"Błąd zapisu: {ex.Message}");
            }
        }


    }
}

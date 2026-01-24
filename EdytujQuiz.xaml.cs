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
                MessageBox.Show("Wybierz pytanie do edycji!");
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
                MessageBox.Show("Podaj nazwę quizu!");
                return;
            }

            if (Questions.Count == 0)
            {
                MessageBox.Show("Quiz musi zawierać przynajmniej jedno pytanie!");
                return;
            }

            // Aktualizujemy obiekt quizu
            _quiz.Title = quizName;
            _quiz.Questions = Questions.ToList();
            _quiz.QuestionsCount = Questions.Count;

            // Folder Quizy w katalogu projektu (lub w Documents, jeśli wolisz bezpiecznie)
            string projectFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string folder = System.IO.Path.Combine(projectFolder, "Quizy");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string filePath = System.IO.Path.Combine(folder, quizName + ".json");

            try
            {
                // 1️⃣ Usuwamy stary plik (jeżeli istnieje)
                if (File.Exists(filePath))
                    File.Delete(filePath);

                // 2️⃣ Zapisujemy nowy plik
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_quiz, options);
                File.WriteAllText(filePath, json);

                MessageBox.Show($"Quiz zapisany do pliku {filePath}");

                // 3️⃣ Wracamy do listy quizów
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                    mainWindow.contentControl.Content = new Quizy();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Brak uprawnień do zapisu w tym katalogu. Spróbuj zmienić folder zapisu lub uruchom aplikację jako administrator.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas zapisu: {ex.Message}");
            }
        }

    }
}

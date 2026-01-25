using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

namespace QuziLab
{

    public partial class Quizy : UserControl
    {
        public ObservableCollection<Quiz> LoadedQuizzes { get; set; } = new ObservableCollection<Quiz>();
        public Quizy()
        {
            InitializeComponent();


            DataContext = this;

            LoadQuizzes();
        }
        private void LoadQuizzes()
        {
            // folder w katalogu projektu
            string projectFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string folder = System.IO.Path.Combine(projectFolder, "Quizy");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            LoadedQuizzes.Clear();

            foreach (string file in Directory.GetFiles(folder, "*.json"))
            {
                try
                {
                    string json = File.ReadAllText(file);
                    Quiz quiz = JsonSerializer.Deserialize<Quiz>(json);

                    if (quiz != null)
                    {
                        quiz.FilePath = file; // ⭐ zapamiętujemy plik
                        LoadedQuizzes.Add(quiz);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd w pliku {file}: {ex.Message}");
                }
            }

        }
        private void NowyQuiz_Click(object sender, RoutedEventArgs e)
        {
            // Szukamy głównego okna i podmieniamy zawartość
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.contentControl.Content = new StworzQuiz();
            }
        }

        private void EdytujQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (QuizListBox.SelectedItem is not Quiz selectedQuiz)
            {
                Alert.Show("Wybierz quiz do edycji!");
                return;
            }

            string projectFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;

            string folder = System.IO.Path.Combine(projectFolder, "Quizy");
            string filePath = System.IO.Path.Combine(folder, $"{selectedQuiz.Title}.json");

            if (!File.Exists(filePath))
            {
                Alert.Show("Nie znaleziono pliku quizu!");
                return;
            }

            string json = File.ReadAllText(filePath);
            Quiz loadedQuiz = JsonSerializer.Deserialize<Quiz>(json);

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
                mainWindow.contentControl.Content = new EdytujQuiz(loadedQuiz);
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            if (QuizListBox.SelectedItem is not Quiz selectedQuiz)
            {
                Alert.Show("Wybierz quiz z listy!");
                return;
            }

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.contentControl.Content =
                    new UstawieniaTestu(selectedQuiz);
            }
        }

        private void Nauka_Click(object sender, RoutedEventArgs e)
        {
            if (QuizListBox.SelectedItem is Quiz quiz)
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow.contentControl.Content = new Nauka(quiz);
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (QuizListBox.SelectedItem is not Quiz selectedQuiz)
            {
                Alert.Show("Wybierz quiz z listy do eksportu!");
                return;
            }

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Plik tekstowy (*.txt)|*.txt";
            saveFileDialog.Title = "Eksportuj quiz (Format Web)";
            saveFileDialog.FileName = $"{selectedQuiz.Title}.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var q in selectedQuiz.Questions)
                    {
                        sb.AppendLine(q.Content); // 1. linia: Pytanie

                        char correctLetter = 'A';

                        for (int i = 0; i < q.Answers.Count; i++)
                        {
                            // Wypisujemy czystą treść odpowiedzi (bez "A. ")
                            sb.AppendLine(q.Answers[i].Content);

                            // Zapamiętujemy literę dla poprawnej odpowiedzi (0=A, 1=B...)
                            if (q.Answers[i].IsCorrect)
                            {
                                correctLetter = (char)('A' + i);
                            }
                        }

                        sb.AppendLine($"correct={correctLetter}"); // Litera poprawnej
                        sb.AppendLine("solution="); // Pusta linia dla kompatybilności
                        sb.AppendLine("---");
                    }

                    File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);
                    Alert.Show("Quiz poprawnie wyeksportowany.");
                }
                catch (Exception ex)
                {
                    Alert.Show($"Błąd podczas eksportu: {ex.Message}");
                }
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Plik tekstowy (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string content = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                    string[] questionBlocks = content.Split(new[] { "---" }, StringSplitOptions.RemoveEmptyEntries);

                    Quiz newQuiz = new Quiz
                    {
                        Title = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName),
                        Author = "Importowany",
                        Questions = new List<Question>()
                    };

                    foreach (var block in questionBlocks)
                    {
                        var lines = block.Trim().Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None)
                                         .Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToList();

                        if (lines.Count < 2) continue;

                        Question q = new Question { Content = lines[0], Answers = new List<Answer>() };
                        string correctValue = "";

                        // Analizujemy linie od drugiej (indeks 1)
                        for (int i = 1; i < lines.Count; i++)
                        {
                            string currentLine = lines[i];

                            if (currentLine.StartsWith("correct="))
                            {
                                correctValue = currentLine.Replace("correct=", "").Trim().ToUpper();
                            }
                            else if (currentLine.StartsWith("solution="))
                            {
                                continue; // Ignorujemy wyjaśnienie
                            }
                            else
                            {
                                // Każda inna linia to po prostu kolejna odpowiedź
                                q.Answers.Add(new Answer { Content = currentLine, IsCorrect = false });
                            }
                        }

                        // Przypisujemy poprawność na podstawie litery (A=0, B=1...)
                        if (!string.IsNullOrEmpty(correctValue) && correctValue.Length > 0)
                        {
                            int correctIndex = correctValue[0] - 'A';
                            if (correctIndex >= 0 && correctIndex < q.Answers.Count)
                            {
                                q.Answers[correctIndex].IsCorrect = true;
                            }
                        }

                        newQuiz.Questions.Add(q);
                    }

                    newQuiz.QuestionsCount = newQuiz.Questions.Count;
                    SaveImportedQuiz(newQuiz);
                    LoadQuizzes();
                    Alert.Show("Import zakończony sukcesem!");
                }
                catch (Exception ex)
                {
                    Alert.Show("Błąd importu: " + ex.Message);
                }
            }
        }

        // Funkcja pomocnicza do zapisu zaimportowanego quizu do folderu JSON
        private void SaveImportedQuiz(Quiz quiz)
        {
            string projectFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string folder = System.IO.Path.Combine(projectFolder, "Quizy");
            string filePath = System.IO.Path.Combine(folder, $"{quiz.Title}.json");

            string json = JsonSerializer.Serialize(quiz, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }


        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            if (QuizListBox.SelectedItem is not Quiz selectedQuiz)
            {
                Alert.Show("Wybierz quiz do usunięcia.");
                return;
            }

            var result = MessageBox.Show(
                $"Czy na pewno chcesz usunąć quiz „{selectedQuiz.Title}”?",
                "Potwierdzenie",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                // usuwamy plik
                if (File.Exists(selectedQuiz.FilePath))
                    File.Delete(selectedQuiz.FilePath);

                // usuwamy z listy (ListBox odświeży się sam)
                LoadedQuizzes.Remove(selectedQuiz);
            }
            catch (Exception ex)
            {
                Alert.Show("Błąd przy usuwaniu quizu:\n" + ex.Message);
            }

        }

      
    }
}


   
    

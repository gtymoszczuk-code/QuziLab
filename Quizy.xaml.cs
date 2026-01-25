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


   
    

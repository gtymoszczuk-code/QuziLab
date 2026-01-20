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
                        LoadedQuizzes.Add(quiz);
                }
                catch (Exception ex)
                {
                    // jeśli plik nie jest poprawnym JSONem
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
    }
}


   
    

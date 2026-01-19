using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
        public Quizy()
        {
            InitializeComponent();

            // 2. LISTĘ ATRAP
            var atrapy = new List<Quiz>
            {
                new Quiz { Title = "Geografia Świata", QuestionsCount = 20 },
                new Quiz { Title = "Historia Polski", QuestionsCount = 15 },
                new Quiz { Title = "Matematyka", QuestionsCount = 10 },
                new Quiz { Title = "Biologia - Komórki", QuestionsCount = 30 }

            };

            // 3. przekazanie listy atrap do ListBoxa
            QuizListBox.ItemsSource = atrapy;
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

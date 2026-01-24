using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Logika interakcji dla klasy PodsumowanieTestu.xaml
    /// </summary>
    public partial class PodsumowanieTestu : UserControl
    {
        public PodsumowanieTestu(string title, int score, int maxScore, TimeSpan testTime)
        {
            InitializeComponent();

            QuizTitle.Text = title;
            FinalTime.Text = testTime.ToString();
            double percent = ((double)score / maxScore) * 100;
            ScoreText.Text = $"{score}/{maxScore} pkt. ({percent:F0}%)";

            // Prosta logika wystawiania oceny
            if (percent >= 90) GradeText.Text = "Bardzo-dobry";
            else if (percent >= 75) GradeText.Text = "Dobry";
            else if (percent >= 50) GradeText.Text = "Dostateczny";
            else GradeText.Text = "Niedostateczny";
        }


        private void RestartTest_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.contentControl.Content = new Quizy();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
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
    public partial class Nauka : UserControl
    {
        private readonly List<Question> _questions;
        private int _index = 0;

        private RadioButton[] _radioButtons;
        public Nauka(Quiz quiz)
        {
            InitializeComponent();
            _questions = quiz.Questions;

            _radioButtons = new[]
            {
            OdpA, OdpB, OdpC, OdpD, OdpE
            }; 

            LoadQuestion();
        }

         private void LoadQuestion()
        {
            if (_index >= 1)
            {
                PowrotBT.IsEnabled = true;
            }
            else
            {
                PowrotBT.IsEnabled = false;
            }
                var q = _questions[_index];

            Counter.Text = $"{_index + 1} / {_questions.Count}";
            QuestionContent.Text = q.Content;

            foreach (var rb in _radioButtons)
            {
                rb.Visibility = Visibility.Collapsed;
                rb.IsChecked = false;
                rb.Tag = null;
            }

            for (int i = 0; i < q.Answers.Count && i < 5; i++)
            {
                _radioButtons[i].Content = q.Answers[i].Content;
                _radioButtons[i].Tag = q.Answers[i];
                _radioButtons[i].Visibility = Visibility.Visible;
            }

            if (_index == _questions.Count - 1)
            {
                Next.Content = "Zakończ Quiz";
            }
            else
            {
                Next.Content = "Następne";
            }
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (_index > 0)
            {
                _index--;
                LoadQuestion();
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (_index == _questions.Count - 1)
            {
                Alert.Show("Ukończono naukę. Wracasz do Quizów");

                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.contentControl.Content = new Quizy();
                }
                return;
            }


            if (_index < _questions.Count - 1)
            {
                _index++;
                LoadQuestion();
            }
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {


            var selected = _radioButtons.FirstOrDefault(r => r.IsChecked == true);
            if (selected == null)
            {
                Alert.Show("Wybierz odpowiedź!");
                return;
            }
            
            // poprawna odpowiedź
            var correctRadio = _radioButtons
                .FirstOrDefault(r => r.Tag is Answer a && a.IsCorrect);


            if (selected != correctRadio && selected.Tag != "Correct")
                selected.Tag = "Wrong";

            if (selected == correctRadio)
                selected.Tag = "Correct";

            

        }




       
    }
}

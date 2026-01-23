using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;


namespace QuziLab
{
    /// <summary>
    /// Logika interakcji dla klasy Test.xaml
    /// </summary>
    public partial class Test : UserControl
    {
        private readonly List<Question> _questions;
        private int _currentIndex = 0;
        private readonly TestSettings _settings;
        private int _score = 0;
        private readonly string _quizTitle;

        private DispatcherTimer _timer;
        private TimeSpan _timeLeft;


        private RadioButton[] _radioButtons;
        public Test(List<Question> questions, TestSettings settings, string quizTitle)
        {
            InitializeComponent();

            _questions = questions;
            _settings = settings;
            _quizTitle = quizTitle;

            _radioButtons = new[]
            {
                OdpA, OdpB, OdpC, OdpD, OdpE
            };
            
            
            StartTimer();
            LoadQuestion();
            
        }
        private void StartTimer()
        {
            _timeLeft = TimeSpan.FromMinutes(_settings.TimeMinutes);

            TimerText.Text = _timeLeft.ToString(@"mm\:ss");

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            _timeLeft = _timeLeft.Subtract(TimeSpan.FromSeconds(1));
            TimerText.Text = _timeLeft.ToString(@"mm\:ss");

            if (_timeLeft.TotalSeconds <= 0)
            {
                _timer.Stop();
                EndTest();
            }

            if (_timeLeft.TotalSeconds <= 10)
            {
                TimerText.Foreground = Brushes.Red;
            }
            else
            {
                TimerText.Foreground = Brushes.Black;
            }
        }

        private void ZapiszWynik()
        {
            try
            {
                // folder projektu
                string projectFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string folder = System.IO.Path.Combine(projectFolder, "Quizy");

                // jeśli folder nie istnieje (zabezpieczenie)
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                // plik obok folderu Quizy
                string filePath = System.IO.Path.Combine(folder, "..", "Wyniki.txt");

                string line = $"{_quizTitle} ; {DateTime.Now:yyyy-MM-dd HH:mm} ; {_score}/{_questions.Count}";

                File.AppendAllText(filePath, line + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisu wyniku: {ex.Message}");
            }
        }
        private void EndTest()
        {
            _timer.Stop();
            ZapiszWynik();

            MessageBox.Show($"⏰ Czas minął!\nTwój wynik: {_score} / {_questions.Count}");

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null && mainWindow.contentControl != null)
            {
                mainWindow.contentControl.Content = new Quizy();
            }


        }


        private void LoadQuestion()
        {
            var q = _questions[_currentIndex];
            Counter.Text = $"{_currentIndex + 1}/{_questions.Count}";
            QuestionContent.Text = q.Content;
            string[] pytania = ["A", "B", "C", "D", "E"];
            // reset
            foreach (var rb in _radioButtons)
            {
                rb.Visibility = Visibility.Collapsed;
                rb.IsChecked = false;
                rb.Tag = null;
            }
            
            // wypełnianie tylko istniejących odpowiedzi
            for (int i = 0; i < q.Answers.Count && i < 5; i++)
            {
                _radioButtons[i].Content = pytania[i]+". "+q.Answers[i].Content;
                _radioButtons[i].Tag = q.Answers[i]; // przechowujemy Answer
                _radioButtons[i].Visibility = Visibility.Visible;
                
            }
        }

        private void FinishBtn_Click(object sender, RoutedEventArgs e)
        {
            // 1. Znajdujemy główne okno aplikacji
            var mainWindow = Window.GetWindow(this) as MainWindow;

            // 2. Jeśli okno istnieje i ma obszar treści, podmieniamy widok
            if (mainWindow != null && mainWindow.contentControl != null)
            {
                // 3. Tworzymy nową instancję strony Quizy i ustawiamy ją jako aktualną
                mainWindow.contentControl.Content = new Quizy();
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            var selected = _radioButtons.FirstOrDefault(r => r.IsChecked == true);

            if (selected == null)
            {
                MessageBox.Show("Wybierz odpowiedź!");
                return;
            }

            var answer = selected.Tag as Answer;
            if (answer != null && answer.IsCorrect)
                _score++;

            _currentIndex++;

            if (_currentIndex >= _questions.Count)
            {
                // koniec testu
                MessageBox.Show($"Koniec testu!\nWynik: {_score} / {_questions.Count}");
                ZapiszWynik();
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null && mainWindow.contentControl != null)
                {
                    mainWindow.contentControl.Content = new Quizy();
                }
                _timer.Stop();
                return; // ważne! kończymy metodę
            }

            // jeśli test się nie skończył, ładujemy kolejne pytanie
            LoadQuestion();
        }



    }
}

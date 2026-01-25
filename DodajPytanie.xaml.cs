using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static QuziLab.Quiz;

namespace QuziLab
{
    public partial class DodajPytanie : UserControl
    {
        private readonly ObservableCollection<Question> _questions;
        private readonly Question _editingQuestion;
        private readonly UserControl _parent;

        public DodajPytanie(UserControl parent, ObservableCollection<Question> questions, Question questionToEdit = null)
        {
            InitializeComponent();
            _parent = parent;
            _questions = questions;
            _editingQuestion = questionToEdit;

            if (_editingQuestion != null)
                LoadQuestion();
        }

        private void LoadQuestion()
        {
            PytanieInput.Text = _editingQuestion.Content;

            var answers = _editingQuestion.Answers;

            if (answers.Count > 0) OdpAInput.Text = answers[0].Content;
            if (answers.Count > 1) OdpBInput.Text = answers[1].Content;
            if (answers.Count > 2) OdpCInput.Text = answers[2].Content;
            if (answers.Count > 3) OdpDInput.Text = answers[3].Content;
            if (answers.Count > 4) OdpEInput.Text = answers[4].Content;

            int index = answers.FindIndex(a => a.IsCorrect);
            if (index >= 0)
                PoprawnaInput.Text = new[] { "a", "b", "c", "d", "e" }[index];
        }

        private List<Answer> BuildAnswers()
        {
            var inputs = new[]
            {
                OdpAInput, OdpBInput, OdpCInput, OdpDInput, OdpEInput
            };

            var answers = new List<Answer>();

            foreach (var tb in inputs)
            {
                if (!string.IsNullOrWhiteSpace(tb.Text))
                {
                    answers.Add(new Answer
                    {
                        Content = tb.Text,
                        IsCorrect = false
                    });
                }
            }

            return answers;
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            var answers = BuildAnswers();
            string correct = PoprawnaInput.Text.Trim().ToLower();

            if (answers.Count < 2)
            {
                Alert.Show("Podaj minimum 2 odpowiedzi.");
                return;
            }

            string[] letters = { "a", "b", "c", "d", "e" };
            int correctIndex = Array.IndexOf(letters, correct);

            if (correctIndex < 0 || correctIndex >= answers.Count)
            {
                Alert.Show("Poprawna odpowiedź musi wskazywać istniejącą literę (a–e).");
                return;
            }

            for (int i = 0; i < answers.Count; i++)
                answers[i].IsCorrect = (i == correctIndex);

            if (_editingQuestion != null)
            {
                _editingQuestion.Content = PytanieInput.Text;
                _editingQuestion.Answers = answers;
            }
            else
            {
                _questions.Add(new Question
                {
                    Content = PytanieInput.Text,
                    Answers = answers
                });
            }

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
                mainWindow.contentControl.Content = _parent;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
                mainWindow.contentControl.Content = _parent;

        }
    }
}

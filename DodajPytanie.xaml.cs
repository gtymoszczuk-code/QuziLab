using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static QuziLab.Quiz;

namespace QuziLab
{
    public partial class DodajPytanie : UserControl
    {
        private readonly StworzQuiz _parent;
        private Question _editingQuestion;

        public DodajPytanie(StworzQuiz parent, Question questionToEdit = null)
        {
            InitializeComponent();
            _parent = parent;

            if (questionToEdit != null)
            {
                _editingQuestion = questionToEdit;
                LoadQuestionIntoUI(questionToEdit);
            }
        }

        private void LoadQuestionIntoUI(Question question)
        {
            PytanieInput.Text = question.Content;

            OdpAInput.Text = question.Answers.ElementAtOrDefault(0)?.Content ?? "";
            OdpBInput.Text = question.Answers.ElementAtOrDefault(1)?.Content ?? "";
            OdpCInput.Text = question.Answers.ElementAtOrDefault(2)?.Content ?? "";
            OdpDInput.Text = question.Answers.ElementAtOrDefault(3)?.Content ?? "";
            OdpEInput.Text = question.Answers.ElementAtOrDefault(4)?.Content ?? "";

            int correctIndex = question.Answers.FindIndex(a => a.IsCorrect);
            if (correctIndex >= 0)
                PoprawnaInput.Text = new[] { "a", "b", "c", "d", "e" }[correctIndex];
        }

        private List<Answer> BuildAnswers()
        {
            var dict = new Dictionary<string, TextBox>
            {
                { "a", OdpAInput },
                { "b", OdpBInput },
                { "c", OdpCInput },
                { "d", OdpDInput },
                { "e", OdpEInput }
            };

            List<Answer> result = new();
            foreach (var kv in dict)
                if (!string.IsNullOrWhiteSpace(kv.Value.Text))
                    result.Add(new Answer { Content = kv.Value.Text, IsCorrect = false });

            return result;
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            var answers = BuildAnswers();
            string correctLetter = PoprawnaInput.Text.Trim().ToLower();

            if (answers.Count < 2)
            {
                MessageBox.Show("Musisz podać przynajmniej 2 odpowiedzi!");
                return;
            }

            int indexCorrect = Array.IndexOf(new[] { "a", "b", "c", "d", "e" }, correctLetter);
            if (indexCorrect < 0 || indexCorrect >= answers.Count)
            {
                MessageBox.Show("Poprawna odpowiedź musi odpowiadać istniejącej odpowiedzi (litera a-e)!");
                return;
            }

            for (int i = 0; i < answers.Count; i++)
                answers[i].IsCorrect = (i == indexCorrect);

            if (_editingQuestion != null)
            {
                _editingQuestion.Content = PytanieInput.Text;
                _editingQuestion.Answers = answers;
                _parent.QuestionsListBox.Items.Refresh();
            }
            else
            {
                _parent.Questions.Add(new Question
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

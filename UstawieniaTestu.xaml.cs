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
    /// Logika interakcji dla klasy UstawieniaTestu.xaml
    /// </summary>
    public partial class UstawieniaTestu : UserControl
    {
        private readonly Quiz _quiz;
        public UstawieniaTestu(Quiz quiz)
        {
            InitializeComponent();
            _quiz = quiz;
            QuestionsCountInput.Text = _quiz.QuestionsCount.ToString();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
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

        private void RozpocznijBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TestTimeInput.Text, out int time) || time <= 0)
            {
                Alert.Show("Podaj poprawny czas testu.");
                return;
            }

            if (!int.TryParse(QuestionsCountInput.Text, out int count) || count <= 0)
            {
                Alert.Show("Podaj poprawną liczbę pytań.");
                return;
            }

            if (count > _quiz.Questions.Count)
            {
                Alert.Show($"Quiz ma tylko {_quiz.Questions.Count} pytań!");
                return;
            }

            // 1️⃣ ustawienia
            TestSettings settings = new TestSettings
            {
                TimeMinutes = time,
                QuestionsCount = count
            };

            // 2️⃣ losowanie pytań
            var questions = _quiz.Questions
                .OrderBy(q => Guid.NewGuid())
                .Take(settings.QuestionsCount)
                .ToList();

            // 3️⃣ przejście do Test
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.contentControl.Content =
                    new Test(questions, settings, _quiz.Title);
            }
        }


        //przycisk inkrementacji inputa
        public void btn_increment(object sender, RoutedEventArgs e)
        {
            // 1. Pobieramy przycisk, który został kliknięty
            Button btn = sender as Button;

            // 2. Szukamy TextBoxa w tym samym kontenerze (np. StackPanelu), w którym jest przycisk
            // Szukamy wśród "rodzeństwa" (rodzic -> dzieci)
            var parent = VisualTreeHelper.GetParent(btn) as FrameworkElement;

            // Jeśli przyciski są w osobnym StackPanelu (np. poziomym), wychodzimy o poziom wyżej
            if (parent is StackPanel && !(parent.Parent is Grid)) parent = parent.Parent as FrameworkElement;

            TextBox target = FindControlInParent<TextBox>(parent);

            if (target != null && int.TryParse(target.Text, out int val) && val < _quiz.QuestionsCount)
            {
                target.Text = (val + 1).ToString();
            }
        }
        //przycisk dekrementacji inputa
        public void btn_decrement(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var parent = VisualTreeHelper.GetParent(btn) as FrameworkElement;

            if (parent is StackPanel && !(parent.Parent is Grid)) parent = parent.Parent as FrameworkElement;

            TextBox target = FindControlInParent<TextBox>(parent);

            if (target != null && int.TryParse(target.Text, out int val) && val > 2)
            {
                target.Text = (val - 1).ToString();
            }
        }

        // funkcja pomocnicza, która znajdzie TextBoxa "obok" przycisku
        private T FindControlInParent<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild) return typedChild;

                // Szukaj głębiej, jeśli TextBox jest np. w Borderze
                var result = FindControlInParent<T>(child);
                if (result != null) return result;
            }
            return null;
        }
    }
    }

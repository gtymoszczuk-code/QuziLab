using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static QuziLab.Quiz;

namespace QuziLab
{
    /// <summary>
    /// Logika interakcji dla klasy EdytujQuiz.xaml
    /// </summary>
    public partial class EdytujQuiz : UserControl
    {
        public EdytujQuiz()
        {
            InitializeComponent();
            DataContext = this; // binding dla ListBox
        }

        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow.contentControl.Content = new Quizy();
        }

        private void DodajPytanie_Click(object sender, RoutedEventArgs e)
        {
           // var mainWindow = Window.GetWindow(this) as MainWindow;
           // mainWindow.contentControl.Content = new DodajPytanie(this);
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            var selected = QuestionsListBox.SelectedItems.Cast<Question>().ToList();
            foreach (var q in selected)
                Questions.Remove(q);
        }

        private void EditBT_Click(object sender, RoutedEventArgs e)
        {
            if (QuestionsListBox.SelectedItem is Question selectedQuestion)
            {
               // var mainWindow = Window.GetWindow(this) as MainWindow;
               // mainWindow.contentControl.Content = new DodajPytanie(this, selectedQuestion);
            }
            else
            {
                MessageBox.Show("Wybierz pytanie do edycji!");
            }
        }

        private void SaveQuiz_Click(object sender, RoutedEventArgs e)
        {
            

        }
    }
}

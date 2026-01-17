using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChangePage(new Start()); // Aplikacja zacznie od strony Start
        }

        private void ChangePage(UserControl newPage)
        {
            contentControl.Content = newPage;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(new Start());
        }

        private void BtnQuizy_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(new Quizy());
        }
        private void BtnWyniki_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(new Wyniki());
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(new About());
        }
    }

}
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
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChangePage(new Start()); // Aplikacja zacznie od strony Start
        }


        //customowy messagebox -----------------------------------------

        // Funkcja wywołująca
        public void ShowCustomMessage(string message)
        {
            MessageBoxText.Text = message;
            CustomMessageBoxOverlay.Visibility = Visibility.Visible;

            // Blokada interakcji z tłem
            contentControl.IsEnabled = false;
        }

        // Funkcja zamykająca
        private void CloseMessageBox_Click(object sender, RoutedEventArgs e)
        {
            CustomMessageBoxOverlay.Visibility = Visibility.Collapsed;
            contentControl.IsEnabled = true;
        }

        // Blokada kliknięć "przez" tło
        private void Block_Click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        // -------------------------------------------------------------


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
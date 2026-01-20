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
    /// Logika interakcji dla klasy Test.xaml
    /// </summary>
    public partial class Test : UserControl
    {
        public Test()
        {
            InitializeComponent();
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


    }


}

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
    
    public partial class Start : UserControl
    {
        public Start()
        {
            InitializeComponent();
        }

        private void BtnGoToQuizy_Click(object sender, RoutedEventArgs e)
        {
            // 1. Znajdujemy główne okno aplikacji
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                // 2. Wywołujemy zmianę strony
                mainWindow.contentControl.Content = new Quizy();
                mainWindow.BtnQuizy.IsChecked = true;

              
            }
        }
    }
}

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
    /// Logika interakcji dla klasy Nauka.xaml
    /// </summary>
    public partial class Nauka : UserControl
    {
        public Nauka()
        {
            InitializeComponent();
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            // 1. Najpierw czyścimy poprzednie wyniki (na wypadek ponownego kliknięcia)
            ResetTags();

            // 2. Symulujemy, że poprawna odpowiedź to OdpB
            RadioButton poprawna = OdpB;

            // 3. Zawsze kolorujemy poprawną odpowiedź na zielono
            poprawna.Tag = "Correct";

            // 4. Sprawdzamy, czy użytkownik zaznaczył coś innego (błędnego)
            // Szukamy zaznaczonego RadioButtona w Twoim kontenerze
            if (OdpA.IsChecked == true && OdpA != poprawna) OdpA.Tag = "Wrong";
            if (OdpB.IsChecked == true && OdpB != poprawna) OdpB.Tag = "Wrong"; // To się nie wywoła, bo B jest poprawne
            if (OdpC.IsChecked == true && OdpC != poprawna) OdpC.Tag = "Wrong";
            if (OdpD.IsChecked == true && OdpD != poprawna) OdpD.Tag = "Wrong";
            if (OdpE.IsChecked == true && OdpE != poprawna) OdpE.Tag = "Wrong";
        }

        // Funkcja pomocnicza do czyszczenia kolorów
        private void ResetTags()
        {
            OdpA.Tag = null;
            OdpB.Tag = null;
            OdpC.Tag = null;
            OdpD.Tag = null;
            OdpE.Tag = null;
        }
    }
}

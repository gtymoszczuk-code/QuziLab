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

    public partial class Wyniki : UserControl
    {
        public Wyniki()
        {
            InitializeComponent();

            List<WynikModel> daneTestowe = new List<WynikModel>
            {
                new WynikModel { NazwaQuizu = "Historia Polski", DataWyniku = "20.01.2024", ZdobytePunkty = 15, MaxPunkty = 20 },
                new WynikModel { NazwaQuizu = "Programowanie C#", DataWyniku = "21.01.2024", ZdobytePunkty = 20, MaxPunkty = 20 },
                new WynikModel { NazwaQuizu = "Bazy Danych", DataWyniku = "22.01.2024", ZdobytePunkty = 12, MaxPunkty = 15 },
                new WynikModel { NazwaQuizu = "Matematyka Dyskretna", DataWyniku = "23.01.2024", ZdobytePunkty = 8, MaxPunkty = 10 }
            };

            // PRZYPISANIE DO LISTY
            ResultsListBox.ItemsSource = daneTestowe;
        }
    }
}

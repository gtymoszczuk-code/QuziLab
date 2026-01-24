using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        private ObservableCollection<WynikModel> Results { get; set; } = new ObservableCollection<WynikModel>();


        public Wyniki()
        {
            InitializeComponent();
            DataContext = this;

            LoadResults();


        }
        private void LoadResults()
        {
            string projectFolder = Directory
                .GetParent(AppDomain.CurrentDomain.BaseDirectory)
                .Parent.Parent.FullName;

            string quizFolder = System.IO.Path.Combine(projectFolder, "Quizy");
            string filePath = System.IO.Path.Combine(quizFolder, "..", "Wyniki.txt");

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Brak pliku Wyniki.txt");
                return;
            }

            Results.Clear();

            var tempResults = new List<WynikModel>();

            foreach (var line in File.ReadAllLines(filePath))
            {
                // NazwaQuizu ; Data ; 4/5
                var parts = line.Split(';');
                if (parts.Length < 3)
                    continue;

                var scoreParts = parts[2].Split('/');
                if (scoreParts.Length != 2)
                    continue;

                if (!int.TryParse(scoreParts[0].Trim(), out int zdobyte))
                    continue;

                if (!int.TryParse(scoreParts[1].Trim(), out int max))
                    continue;

                tempResults.Add(new WynikModel
                {
                    NazwaQuizu = parts[0].Trim(),
                    DataWyniku = parts[1].Trim(),
                    PunktyZdobyte = zdobyte,
                    PunktyMax = max
                });
            }

            foreach (var r in tempResults
                .OrderByDescending(r => (double)r.PunktyZdobyte / r.PunktyMax)
                .ThenByDescending(r => r.PunktyMax))
            {
                Results.Add(r);
            }

            ResultsListBox.ItemsSource = Results;
        }



    }
}


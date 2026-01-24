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
            string projectFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string resultsFile = System.IO.Path.Combine(projectFolder, "Wyniki.txt");

            if (!File.Exists(resultsFile))
            {
                MessageBox.Show("Plik Wyniki.txt nie istnieje!");
                return;
            }

            Results.Clear();

            var lines = File.ReadAllLines(resultsFile);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                try
                {
                    // spodziewany format: NazwaQuizu ; Data ; PunktyZdobyte/PunktyMax
                    var parts = line.Split(';');
                    if (parts.Length >= 3)
                    {
                        Results.Add(new WynikModel
                        {
                            NazwaQuizu = parts[0].Trim(),
                            DataWyniku = parts[1].Trim(),
                            PunktyDisplay = parts[2].Trim()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd przy parsowaniu linii: {line}, {ex.Message}");
                }
            }

            ResultsListBox.ItemsSource = Results;
        }


    }
}


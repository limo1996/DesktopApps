using System;
using System.Collections.Generic;
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

namespace DUS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Graph graph;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = "";
            (sender as TextBox).Foreground = new SolidColorBrush(Colors.Black);
            (sender as TextBox).GotFocus -= TextBox_GotFocus;

        }

        private void Load(object sender, RoutedEventArgs e)
        {
            string path = InputBox.Text;
            if (File.Exists(path))
            {
                graph = new Graph(XMLParser.Parse(path));
            }
            else
            {
                MessageBox.Show(this, "Zadany subor neexistuje !", "Chyba");
            }
        }

        private void WrongContinuations_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GrafDosiahnutelnosti(object sender, RoutedEventArgs e)
        {
            string graf = graph.Reachability();
            List<Item> items = new List<Item>();
            if (graf != null && graf != "")
            {
                var parsed = graf.Split('\n');
                if (parsed != null && parsed.Length > 1)
                {
                    
                    items.Add(new Item() { From = "", To = "", Tokens = parsed[0] });

                    for (int i = 1; i < parsed.Length; i+=2)
                    {
                        if (parsed[i] != null && parsed[i] != "")
                        {
                            var parsed2 = parsed[i].Split(' ');
                            items.Add(new Item() { From = parsed2[0], To = parsed2[1], Tokens = parsed[i + 1] });
                        }
                    }
                }
            }

            Display2.ItemsSource = items;
        }

        private void Incidency_Click(object sender, RoutedEventArgs e)
        {
            Display1.Text = graph.PrintGraph(Matrix.Incidency);
        }

        private void Output_Click(object sender, RoutedEventArgs e)
        {
            Display1.Text = graph.PrintGraph(Matrix.Output);
        }

        private void Input_Click(object sender, RoutedEventArgs e)
        {
            Display1.Text = graph.PrintGraph(Matrix.Input);
        }

        private void PInvariant_click(object sender, RoutedEventArgs e)
        {
            var tmp = graph.PInvariant();

            if (tmp != null)
                Invariants.Text = tmp.ToString();
            else
                Invariants.Text = "Neexistuje";
        }

        private void TInvariant(object sender, RoutedEventArgs e)
        {
            var tmp = graph.TInvariant1();

            if (tmp != null)
                Invariants.Text = tmp.ToString();
            else
                Invariants.Text = "Neexistuje";
        }

        private void TInvariant2(object sender, RoutedEventArgs e)
        {
            var tmp = graph.TInvariant2();

            if (tmp != null)
                Invariants.Text = tmp.ToString();
            else
                Invariants.Text = "Neexistuje";
        }
    }
}
